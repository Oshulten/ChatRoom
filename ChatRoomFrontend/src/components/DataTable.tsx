import React, { useState } from 'react'
import { lengthPattern } from '../utilities/regexPatterns';

function validateWithPattern(text: string, pattern?: string) {
    if (pattern) {
        return text.match(pattern) != undefined;
    }
    return true;
}

interface StringDataCellProps {
    value: string,
    onChange: (value: string) => void;
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

// interface UserDataRowProps {
// }

export function UserDataRow() {
    const [user, setUser] = useState({
        id: "1",
        alias: "Adam"
    });

    return (<StringDataCell value={user.id} onChange={(newValue) => {
        setUser({ ...user, id: newValue });
        console.log("New value: " + newValue);
    }
    } validationPattern={lengthPattern(0, 4)}></StringDataCell>)
}