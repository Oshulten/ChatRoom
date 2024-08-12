/* eslint-disable react/react-in-jsx-scope */
import { castStringToObject, typeCheck } from "../utilities/casting";
import ObjectInspector from "./objectInspector";

export type InteractiveDataCellSupportedTypes = number | string | boolean | null | undefined | Date | object;

interface InteractiveDataCellProps {
    value: InteractiveDataCellSupportedTypes,
    onChange: (newValue: InteractiveDataCellSupportedTypes) => void,
    validity?: string,
    disabled?: boolean
}

export function InteractiveDataCell({ value, onChange, validity, disabled }: InteractiveDataCellProps) {
    const typeInfo = typeCheck(value);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let properValue = e.target.value;
        if (typeInfo == "boolean") {
            properValue = String(e.target.checked);
        }
        const restoredTypedValue = castStringToObject(properValue, typeInfo);
        onChange(restoredTypedValue);
    }

    const handleChangeObject = (newObject: InteractiveDataCellSupportedTypes) => {
        onChange(newObject);
    }

    const validationClass = validity ? "input-error" : "";

    if (typeInfo === "number") {
        const numberValue = value as number;
        return <input
            type="number"
            placeholder="Number"
            value={String(numberValue)}
            disabled={disabled}
            onChange={e => handleChange(e)}
            className={`input w-full max-w-xs ${validationClass}`} />
    }

    if (typeInfo === "string" || typeInfo === "null" || typeInfo === "undefined") {
        const stringValue = value as string;
        return <input
            type="text"
            placeholder="String"
            value={stringValue}
            disabled={disabled || typeInfo == "null" || typeInfo == "undefined"}
            onChange={e => handleChange(e)}
            className={`input w-full max-w-xs ${validationClass}`} />
    }

    if (typeInfo === "boolean") {
        const booleanValue = value as boolean;
        return <input
            type="checkbox"
            className={`toggle ${validationClass}`}
            disabled={disabled}
            onChange={e => handleChange(e)}
            checked={booleanValue} />
    }

    if (typeInfo === "Date") {
        const dateValue = value as Date;

        const dateTimeElement =
            <input
                type="datetime-local"
                placeholder="Time"
                value={dateValue.toISOString().slice(0, -5)}
                disabled={true}
                onChange={e => handleChange(e)}
                className={`input w-full max-w-xs ${validationClass}`} />

        return <>{dateTimeElement}</>;
    }

    return (
        <ObjectInspector
            subject={value as object}
            subjectKey={""}
            onChange={(newObject) => handleChangeObject(newObject)} />
    );
}