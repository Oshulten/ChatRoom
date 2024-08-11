import { castStringToObject } from "../utilities/casting";
import { toIsoString } from "../utilities/dateRepresentation";
import { typeCheck } from "../utilities/typeCheck";
import ObjectInspector from "./objectInspector";
import { GenericIdEntity } from "../types/genericIdEntity";
import useConnectToDbTable from "../hooks/useConnectToDbTable";

/* eslint-disable react/react-in-jsx-scope */

interface InteractiveDataTableProps {
    endpoint: string,
    label: string,
    disableIds?: boolean;
}

export function InteractiveDataTable<T extends GenericIdEntity>({ endpoint, label, disableIds }: InteractiveDataTableProps) {
    const [entities, setEntities, status] = useConnectToDbTable<T>(endpoint);

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
                                    disableIds={disableIds} />
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
    disableIds?: boolean;
}

export function InteractiveDataRow<T extends GenericIdEntity>({ entity, onChange, disableIds }: InteractiveDataRowProps<T>) {
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
                            disabled={disabled} />
                    </td>
                );
            })}
        </tr>
    );
}

export type InteractiveDataCellSupportedTypes = number | string | boolean | null | undefined | Date | object;

interface InteractiveDataCellProps {
    value: InteractiveDataCellSupportedTypes,
    onChange: (newValue: InteractiveDataCellSupportedTypes) => void,
    disabled?: boolean
}

export function InteractiveDataCell({ value, onChange, disabled }: InteractiveDataCellProps) {
    const typeInfo = typeCheck(value);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let properValue = e.target.value;
        if (typeInfo == "boolean") {
            properValue = String(e.target.checked);
        }
        const restoredTypedValue = castStringToObject(properValue, typeInfo);
        console.log(`onChange in InteractiveDataCell - '${JSON.stringify(restoredTypedValue)}' [${typeCheck(restoredTypedValue)}]`)
        onChange(restoredTypedValue);
    }

    const handleChangeObject = (newObject: InteractiveDataCellSupportedTypes) => {
        console.log(`${newObject}`);
        onChange(newObject);
    }

    if (typeInfo === "number") {
        const numberValue = value as number;
        return <input
            type="number"
            placeholder="Number"
            value={String(numberValue)}
            disabled={disabled}
            onChange={e => handleChange(e)}
            className="input w-full max-w-xs" />
    }
    if (typeInfo === "string" || typeInfo === "null" || typeInfo === "undefined") {
        const stringValue = value as string;
        return <input
            type="text"
            placeholder="String"
            value={stringValue}
            disabled={disabled || typeInfo == "null" || typeInfo == "undefined"}
            onChange={e => handleChange(e)}
            className="input w-full max-w-xs" />
    }
    if (typeInfo === "boolean") {
        const booleanValue = value as boolean;
        return <input
            type="checkbox"
            className="toggle"
            disabled={disabled}
            onChange={e => handleChange(e)}
            checked={booleanValue} />
    }
    if (typeInfo === "Date") {
        const dateValue = value as Date;

        const isoString = toIsoString(dateValue);
        console.log(`ISO Time: ${isoString}`)

        const [year, month, date] = [dateValue.getFullYear(), dateValue.getMonth() + 1, dateValue.getDate()];
        const dateString = `${year}-${month >= 10 ? month : "0" + month}-${date}`;

        const [hours, minutes, seconds] = [dateValue.getHours(), dateValue.getMinutes(), dateValue.getSeconds()];
        const timeString = `${hours >= 10 ? hours : "0" + hours}:${minutes >= 10 ? minutes : "0" + minutes}:${seconds >= 10 ? seconds : "0" + seconds}`;

        const dateTimeString = dateString + "T" + timeString
        console.log(`${dateTimeString}`);

        const dateTimeElement =
            <input
                type="datetime-local"
                placeholder="Time"
                value={dateTimeString}
                disabled={true}
                onChange={e => handleChange(e)}
                className="input w-full max-w-xs" />

        return <>{dateTimeElement}</>;
    }
    return (
        <ObjectInspector
            subject={value as object}
            subjectKey={""}
            onChange={(newObject) => handleChangeObject(newObject)} />
    );
}