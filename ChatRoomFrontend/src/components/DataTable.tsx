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
    onChange: (value?: string) => void;
    validationPattern?: string,
    placeholder?: string,
    validationError?: string,
}

export function StringDataCell({ value, onChange, validationPattern, validationError, placeholder }: StringDataCellProps) {
    const [inputIsValid, setInputIsValid] = useState(false);

    validationError ??= "(Sorry, something is wrong with the input!)";
    placeholder ??= "Enter text...";

    function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        const currentInputIsValid = validateWithPattern(e.target.value, validationPattern);
        const currentValue = e.target.value;
        setInputIsValid(currentInputIsValid);
        onChange(currentValue);
    }

    const inputElement = <input
        type="text"
        value={value}
        required
        placeholder={placeholder}
        onChange={(e) => handleChange(e)}
        className={`input input-bordered w-full max-w-xs ${!inputIsValid ? "input-error" : "input-success"}`}>
    </ input >

    return inputElement;
}