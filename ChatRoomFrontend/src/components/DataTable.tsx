import React from 'react'
import { castStringToPrimitive } from '../utilities/casting';
import type { PrimitiveType } from '../utilities/casting';
import { GenericIdEntity } from '../types/genericIdEntity';
import useConnectToDbTable from '../hooks/useConnectToDbTable';

interface PrimitiveDataTableProps {
    endpoint: string,
    showId: boolean
}

export default function PrimitiveDataTable<T extends GenericIdEntity>({ endpoint, showId }: PrimitiveDataTableProps) {
    const [entities, setEntities, status, validities] = useConnectToDbTable<T>(endpoint);

    function handleChangeFactory(entityId: string) {
        return (newEntity: T) => {
            const entity = entities.find(entity => entity.id == entityId);
            if (entity == undefined) {
                throw new Error(`Entity id ${entityId} cannot be found in state`);
            }
            const entityIndex = entities.indexOf(entity);
            const newEntities = [...entities.slice(0, entityIndex), newEntity!, ...entities.slice(entityIndex + 1)];
            setEntities(newEntities);
        }
    }

    switch (status) {
        case "fetching":
            return <span className="loading loading-spinner loading-lg"></span>;

        case "failure":
            return <>
                <h2 className="text-red-500 text-3xl">Server is asleep...</h2>
                <p className="text-red-900">Terrible sorry! We hope you have wonderful day despite this.</p>
            </>

        case "success":
            return (<>
                <div className="overflow-x-auto">
                    <table className="table table-xs">
                        <thead>
                            <tr>
                                {Object.keys(entities[0]).map((key) => {
                                    if (!showId && key == "id") return;
                                    return (<th key={key}>{key}</th>)
                                }
                                )}
                            </tr>
                        </thead>
                        <tbody>
                            {entities.map(entity => {
                                return <PrimitiveDataRow<T>
                                    showId={showId}
                                    key={entity.id}
                                    entity={entity}
                                    handleChange={handleChangeFactory(entity.id)}
                                    validities={validities[entity.id]} />
                            })}
                        </tbody>
                    </table>
                </div>
            </>)
    }
}

interface CellValidities {
    [key: string]: string
}

interface PrimitiveDataRowProps<T> {
    entity: T,
    showId: boolean,
    handleChange: (newValue: T) => void,
    validities: CellValidities
}

function PrimitiveDataRow<T extends GenericIdEntity>({ entity, showId, handleChange, validities }: PrimitiveDataRowProps<T>) {
    function handleChangeFactory(propertyKey: keyof T) {
        type BlankSlate = {
            [key: string]: PrimitiveType
        }

        return (newPropertyValue: PrimitiveType) => {
            const filledSlate = Object.entries({ ...entity } as BlankSlate).reduce((accumulator, [key, value]) => {
                if (propertyKey == key) {
                    accumulator[key] = newPropertyValue;
                    return accumulator;
                }
                accumulator[key] = value;
                return accumulator;
            }, {} as BlankSlate);

            const newEntity = (filledSlate as unknown) as T;
            handleChange(newEntity);
        }
    }

    return (
        <tr key={entity.id}>
            {Object.keys(entity).map((key) => {
                if (key == "id" && !showId) return;
                return (
                    <td key={key}>
                        <PrimitiveDataCell
                            validity={validities[key]}
                            key={key}
                            value={entity[key]}
                            onChange={handleChangeFactory(key)}
                            disabled={key == "id"} />
                    </td>
                )
            })}
        </tr>
    )
}

interface PrimitiveDataCellProps {
    value: PrimitiveType,
    disabled: boolean,
    onChange: (value: PrimitiveType) => void
    validity: string
}

function PrimitiveDataCell({ value, onChange, disabled, validity }: PrimitiveDataCellProps) {
    const handleChange = (currentString: string) => onChange(castStringToPrimitive(currentString, value));

    let inputElement;
    const validityModifier = validity == "" ? "input-success" : "input-error";
    if (typeof (value) === "string") {
        inputElement =
            <input
                type="text"
                className={`input input-bordered w-full max-w-xs ${validityModifier}`}
                value={value.toString()}
                onChange={(e) => handleChange(e.target.value)}
                disabled={disabled} />
    }

    if (typeof (value) === "boolean") {
        inputElement =
            <select
                className={`select select-bordered w-full max-w-xs ${validityModifier}`}
                onChange={(e) => handleChange(e.target.value)}
                defaultValue={value.toString()}
                disabled={disabled}>
                <option value="true">True</option>
                <option value="false">False</option>
            </select>
    }

    if (typeof (value) === "number") {
        inputElement =
            <input
                type="number"
                className={`${validityModifier} bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500`}
                value={value.toString()}
                disabled={disabled}
                onChange={(e) => handleChange(e.target.value)} />
    }

    return inputElement;
}