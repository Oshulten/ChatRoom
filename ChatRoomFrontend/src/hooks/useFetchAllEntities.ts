import { useEffect, useState } from "react";
import { GenericIdEntity } from "../types/genericIdEntity";

type FetchStatus = "fetching" | "success" | "failure";

export default function useFetchAllEntities<T extends GenericIdEntity>(endpointUrl: string): [T[], React.Dispatch<React.SetStateAction<T[]>>, FetchStatus] {
    const [entities, setEntities] = useState<T[]>([]);
    const [status, setStatus] = useState<FetchStatus>("fetching");

    useEffect(() => {
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
            } catch (error) {
                console.error(`Something went wrong on the backend ${(error as Error).message}`);
                setStatus("failure");
            }
        }
        fetchAllEntities();
    }, []);

    return [entities, setEntities, status];
}