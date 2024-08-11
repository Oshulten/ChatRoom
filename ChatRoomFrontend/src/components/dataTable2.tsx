import { ChatSpaceClass } from "../types/chatSpace";
import { castStringToObject } from "../utilities/casting";
import { toIsoString } from "../utilities/dateRepresentation";
import { typeCheck } from "../utilities/typeCheck";
import { useState } from "react";
import ObjectInspector from "./objectInspector";

/* eslint-disable react/react-in-jsx-scope */
interface InteractiveDataRowProps {
    disableIds?: boolean;
}

export function InteractiveDataRow({ disableIds }: InteractiveDataRowProps) {
    const [entity, setEntity] = useState(ChatSpaceClass.fromProperties("global", "Global", ["Mike", "Adam", "Catherine"]));

    disableIds ??= true;

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
            return (entity.fromObject as Function)(filledSlate as object);
        }
        return filledSlate as object;
    }

    const handleChangeFactory = (propertyKey: string) => {
        return (newPropertyValue: InteractiveDataCellSupportedTypes) => {
            console.log(`onChange in InteractiveDataRow from InteractiveDataCell - '${propertyKey}': ${newPropertyValue}`);
            const newEntity = reassembleEntity(entity, newPropertyValue, propertyKey) as ChatSpaceClass;
            setEntity(newEntity);
        };
    };

    return (
        <div className="overflow-x-auto">
            <table className="table table-xs">
                <thead>
                    <tr>
                        {Object.keys(entity).map((key) => {
                            return (<th key={key}>{key}</th>)
                        })}
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        {Object.entries(entity).map(([key, value]) => {
                            const disabled = disableIds && key.includes("id");
                            return (
                                <td key={key}>
                                    <InteractiveDataCell
                                        value={value}
                                        onChange={newValue => handleChangeFactory(key)(newValue)}
                                        disabled={disabled} />
                                </td>
                            )
                        })}
                    </tr>
                </tbody>
            </table>
            <p>{JSON.stringify(entity)}</p>
        </div>
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

    const handleChangeObject = (newObject: InteractiveDataCellSupportedTypes, key: string) => {
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
        <div className="collapse bg-base-200">
            <input type="checkbox" />
            <div className="collapse-title text-xl font-medium">Click me to show/hide content</div>
            <div className="collapse-content">
                <ObjectInspector
                    subject={value as object}
                    subjectKey={"?"}
                    onChange={(newObject, key) => handleChangeObject(newObject, key)} />
            </div>
        </div>
    )
}