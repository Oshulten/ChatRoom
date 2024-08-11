import { castStringToObject } from "../utilities/casting";
import { typeCheck } from "../utilities/typeCheck";
import ObjectInspector from "./objectInspector";

/* eslint-disable react/react-in-jsx-scope */
interface ValidationInfo {
    message: string;
}

type HandledTypes = number | string | boolean | null | undefined | Date | object;

interface InteractiveDataCellProps {
    value: HandledTypes,
    validation?: ValidationInfo
    onChange?: (newValue: HandledTypes) => void,
    disabled?: boolean
}

export default function InteractiveDataCell({ value, validation, onChange, disabled }: InteractiveDataCellProps) {
    const typeInfo = typeCheck(value);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (onChange) {
            let properValue = e.target.value;
            if (typeInfo == "boolean") {
                properValue = String(e.target.checked);
            }
            const restoredTypedValue = castStringToObject(properValue, typeInfo);
            onChange(restoredTypedValue);
        }
    }

    if (typeInfo === "number" || typeInfo === "null" || typeInfo === "undefined") {
        const numberValue = value as number;
        return <input
            type="number"
            placeholder="Number"
            value={String(numberValue)}
            disabled={disabled || typeInfo == "null" || typeInfo == "undefined"}
            onChange={e => handleChange(e)}
            className="input w-full max-w-xs" />
    }
    if (typeInfo === "string") {
        const stringValue = value as string;
        return <input
            type="text"
            placeholder="String"
            value={stringValue}
            disabled={disabled}
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
        const [year, month, date] = [dateValue.getFullYear(), dateValue.getMonth(), dateValue.getDate()];
        const dateString = `${year}-${month >= 10 ? month : "0" + month}-${date}`;

        const [hours, minutes, seconds] = [dateValue.getHours(), dateValue.getMinutes(), dateValue.getSeconds()];
        const timeString = `${hours >= 10 ? hours : "0" + hours}:${minutes >= 10 ? minutes : "0" + minutes}:${seconds >= 10 ? seconds : "0" + seconds}`;

        const dateTimeElement =
            <input
                type="datetime-local"
                placeholder="Time"
                value={dateString + "T" + timeString}
                disabled={disabled}
                onChange={e => handleChange(e)}
                className="input w-full max-w-xs" />

        return <>{dateTimeElement}</>;
    }
    return <ObjectInspector subject={value as object} subjectKey={"?"} />
}