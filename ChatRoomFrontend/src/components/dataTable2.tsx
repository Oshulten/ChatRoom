import { ChatSpaceClass } from "../types/chatSpace";
import { castStringToObject } from "../utilities/casting";
import { toIsoString } from "../utilities/dateRepresentation";
import { typeCheck } from "../utilities/typeCheck";

/* eslint-disable react/react-in-jsx-scope */
interface InteractiveDataRowProps {
    disableIds?: boolean;
}

export function InteractiveDataRow({ disableIds }: InteractiveDataRowProps) {
    const mockEntity = ChatSpaceClass.fromProperties("global", "Global", ["Mike", "Adam", "Catherine"])
    disableIds ??= true;

    const handleChange = (newValue: InteractiveDataCellSupportedTypes) => {
        console.log(`onChange in InteractiveDataRow from InteractiveDataCell - ${newValue}`);
    };

    return (
        <div className="overflow-x-auto">
            <table className="table table-xs">
                <thead>
                    <tr>
                        {Object.keys(mockEntity).map((key) => {
                            return (<th key={key}>{key}</th>)
                        })}
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        {Object.entries(mockEntity).map(([key, value]) => {
                            const disabled = disableIds && key.toLowerCase().includes("id");
                            return (
                                <td key={key}>
                                    <InteractiveDataCell
                                        value={value}
                                        onChange={(newValue) => handleChange(newValue)}
                                        disabled={disabled} />
                                </td>
                            )
                        })}
                    </tr>
                </tbody>
            </table>
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
    return <button>Inspect</button>
}