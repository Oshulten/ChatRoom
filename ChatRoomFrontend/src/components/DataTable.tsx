import React, { useState } from 'react'


interface StringDataCellProps {
    initialValue: string,
    validationPattern?: string,
    placeholder?: string
}

export default function StringDataCell({ initialValue, validationPattern = undefined, placeholder: placeholder = "Enter text..." }: StringDataCellProps) {
    const [inputIsValid, setInputIsValid] = useState(false);

    function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        validate(e);
    }

    function validate(e: React.ChangeEvent<HTMLInputElement>) {
        const rawValue = e.target.value;
        console.log("---");
        console.log(`Raw value: ${rawValue}`);
        if (validationPattern) {
            setInputIsValid(rawValue.match(validationPattern) != undefined);
        }
    }

    return <input
        type="text"
        defaultValue={initialValue}
        required
        placeholder={placeholder}
        pattern={validationPattern}
        onChange={(e) => handleChange(e)}
        className={`input input-bordered w-full max-w-xs ${!inputIsValid ? "input-error" : "input-success"}`}>
    </ input >
}