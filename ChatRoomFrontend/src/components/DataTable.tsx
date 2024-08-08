import React, { useState } from 'react'

const lengthPattern = (min: number, max: number) => `^.{${min},${max}}$`;
const maxLengthPattern = (max: number) => lengthPattern(0, max);
const minLengthPattern = (min: number) => `^.{${min},}$`;
export { lengthPattern, maxLengthPattern, minLengthPattern };

function validateWithPattern(text: string, pattern?: string) {
    if (pattern) {
        return text.match(pattern) != undefined;
    }
    return true;
}

interface StringDataCellProps {
    initialValue: string,
    validationPattern?: string,
    placeholder?: string,
    validationError?: string,
}

export function StringDataCell({ initialValue, validationPattern, validationError, placeholder }: StringDataCellProps) {
    const [inputIsValid, setInputIsValid] = useState(false);
    const [value, setValue] = useState(initialValue);

    validationError ??= "(Sorry, something is wrong with the input!)";
    placeholder ??= "Enter text...";

    function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        setInputIsValid(validateWithPattern(e.target.value, validationPattern));
        setValue(e.target.value);
    }

    const inputElement = <input
        type="text"
        defaultValue={initialValue}
        value={value}
        required
        placeholder={placeholder}
        onChange={(e) => handleChange(e)}
        className={`input input-bordered w-full max-w-xs ${!inputIsValid ? "input-error" : "input-success"}`}>
    </ input >

    return inputElement;
}