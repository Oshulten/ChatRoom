import { useEffect, useState } from "react";
import { GenericIdEntity } from "../types/genericIdEntity";

type FetchStatus = "fetching" | "success" | "failure";

export interface DataTableValidity {
    [id: string]: DataRowValidity
}

export interface DataRowValidity {
    [key: string]: string
}

export default function useConnectToDbTable<T extends GenericIdEntity>(endpointUrl: string): [T[], React.Dispatch<React.SetStateAction<T[]>>, FetchStatus, DataTableValidity] {
    const [entities, setEntities] = useState<T[]>([]);
    const [status, setStatus] = useState<FetchStatus>("fetching");
    const [dataValidity, setDataValidity] = useState<DataTableValidity>({});

    const fetchAllEntities = async () => {
        setStatus("fetching");
        try {
            const response = await fetch(endpointUrl);
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }
            const json = await response.json() as T[];
            setEntities(json);
            setStatus("success");
            setDataValidity(emptyDataValidity(json));
        } catch (error) {
            console.error(`Something went wrong on the backend ${(error as Error).message}`);
            setStatus("failure");
        }
    }

    const patchEntities = async () => {
        const newValidities = { ...dataValidity }

        entities.forEach(async entity => {
            const url = `${endpointUrl}/${entity.id}`;
            try {
                const response = await fetch(url, {
                    method: "PATCH",
                    body: JSON.stringify(entity),
                    headers: {
                        "Content-Type": "application/json",
                    },

                });
                if (!response.ok) {
                    const content = await response.json();
                    Object.keys(entities[0]).forEach(key => {
                        const capitalizedKey = `${key.charAt(0).toUpperCase()}${key.slice(1)}`;
                        if (content.errors[capitalizedKey]) {
                            newValidities[entity.id][key] = content.errors[capitalizedKey][0];
                        }
                        else {
                            newValidities[entity.id][key] = "";
                        }
                    });
                    throw new Error(`Response status: ${response.status}`);
                } else {
                    Object.keys(entities[0]).forEach(key => {
                        newValidities[entity.id][key] = "";
                    });
                }
            } catch (error) {
                console.error(`Patch error: ${(error as Error).message}`);
            }
            setDataValidity(newValidities);
        })
    }

    function emptyDataValidity(entities: T[]) {
        const newValidities: DataTableValidity = {};
        entities.forEach(entity => {
            newValidities[entity.id] = {};
            Object.keys(entities[0]).forEach(key => {
                newValidities[entity.id][key] = "";
            })
        });
        return newValidities;
    }

    useEffect(() => {
        fetchAllEntities();
    }, []);

    useEffect(() => {
        patchEntities();
    }, [entities]);

    return [entities, setEntities, status, dataValidity];
}