import { GenericIdEntity } from "../types/genericIdEntity";
import useConnectToDbTable, { DataRowValidity } from "../hooks/useConnectToDbTable";
import { InteractiveDataCell, InteractiveDataCellSupportedTypes } from "./interactiveDataCell";

/* eslint-disable react/react-in-jsx-scope */

interface InteractiveDataTableProps {
    endpoint: string,
    label: string,
    disableIds?: boolean;
}

export function InteractiveDataTable<T extends GenericIdEntity>({ endpoint, label, disableIds }: InteractiveDataTableProps) {
    const [entities, setEntities, status, validity] = useConnectToDbTable<T>(endpoint);
    disableIds ??= true;

    const handleChange = (newEntity: GenericIdEntity) => {
        if (newEntity == undefined) {
            throw new Error(`Entity cannot be found in state`);
        }

        const oldEntity = entities.find((entityFromList) => entityFromList.id == newEntity.id) as T;
        const entityIndex = entities.indexOf(oldEntity);
        const newEntities = [...entities.slice(0, entityIndex), newEntity!, ...entities.slice(entityIndex + 1)];

        setEntities(newEntities as T[]);
    }

    let content;
    switch (status) {
        case "fetching":
            content = <span className="loading loading-spinner loading-lg"></span>;
            break;

        case "failure":
            content = <>
                <h2 className="text-red-500 text-3xl">Server is asleep...</h2>
                <p className="text-red-900">Terrible sorry! We hope you have wonderful day despite this.</p>
            </>;
            break;

        case "success":
            content = (
                <div className="overflow-x-auto">
                    <table className="table table-xs">
                        <thead>
                            <tr>
                                {Object.keys(entities[0]).map((key) => {
                                    return (<th key={key}>{key}</th>)
                                })}
                            </tr>
                        </thead>
                        <tbody>
                            {Object.values(entities).map(entity => {
                                return <InteractiveDataRow<T>
                                    key={entity.id}
                                    entity={entity}
                                    onChange={(entity) => handleChange(entity)}
                                    disableIds={disableIds}
                                    validity={validity[entity.id]} />
                            })}
                        </tbody>
                    </table>
                </div>
            )
            break;
    }
    return (
        <>
            <div className="rounded-lg bg-gray-600 p-4">
                <h2>{label}</h2>
                {content}
            </div>
        </>
    );
}

interface InteractiveDataRowProps<T extends GenericIdEntity> {
    entity: T;
    onChange: (entity: T) => void;
    validity?: DataRowValidity,
    disableIds?: boolean;
}

export function InteractiveDataRow<T extends GenericIdEntity>({ entity, onChange, validity, disableIds }: InteractiveDataRowProps<T>) {
    const reassembleEntity = (entity: object, newPropertyValue: InteractiveDataCellSupportedTypes, propertyKey: string) => {
        type BlankSlate = {
            [key: string]: InteractiveDataCellSupportedTypes
        }

        const filledSlate = Object.entries({ ...entity } as BlankSlate).reduce((accumulator, [key, value]) => {
            if (propertyKey == key) {
                accumulator[key] = newPropertyValue;
                return accumulator;
            }
            accumulator[key] = value;
            return accumulator;
        }, {} as BlankSlate);

        if ("fromObject" in entity) {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-function-type
            return (entity.fromObject as Function)(filledSlate as T);
        }
        return filledSlate as T;
    }

    const handleChangeFactory = (propertyKey: string) => {
        return (newPropertyValue: InteractiveDataCellSupportedTypes) => {
            const newEntity = reassembleEntity(entity, newPropertyValue, propertyKey);
            onChange(newEntity);
        };
    };

    return (
        <tr key={entity.id}>
            {Object.entries(entity).map(([key, value]) => {
                const disabled = disableIds && key.includes("id");
                return (
                    <td key={key}>
                        <InteractiveDataCell
                            value={value}
                            onChange={newValue => handleChangeFactory(key)(newValue)}
                            disabled={disabled}
                            validity={validity?.[key]} />
                    </td>
                );
            })}
        </tr>
    );
}

