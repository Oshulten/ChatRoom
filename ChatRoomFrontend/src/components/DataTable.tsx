import React, { useState } from 'react'
import { lengthPattern } from '../utilities/regexPatterns';
import User from '../models/User';

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
    const [inputIsValid, setInputIsValid] = useState(validateWithPattern(value, validationPattern));

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
    const [users, setUsers] = useState<User[]>([{
        id: "1",
        alias: "Adam"
    },
    {
        id: "2",
        alias: "Bertha"
    }]);

    const handleChangeIdFirst = (newValue: string) => {
        const newUser: User = { ...users[0], id: newValue };
        const newUsers = [newUser, ...users.slice(1, 2)];
        setUsers(newUsers);
        console.log(newUsers);
    }

    const handleChangeAliasFirst = (newValue: string) => {
        const newUser: User = { ...users[0], alias: newValue };
        const newUsers = [newUser, ...users.slice(1, 2)];
        setUsers(newUsers);
        console.log(newUsers);
    }

    const handleChangeIdSecond = (newValue: string) => {
        const newUser: User = { ...users[1], id: newValue };
        const newUsers = [...users.slice(0, 1), newUser];
        setUsers(newUsers);
        console.log(newUsers);
    }

    const handleChangeAliasSecond = (newValue: string) => {
        const newUser: User = { ...users[1], alias: newValue };
        const newUsers = [...users.slice(0, 1), newUser];
        setUsers(newUsers);
        console.log(newUsers);
    }

    return (
        <>
            <div className="overflow-x-auto">
                <table className="table table-xs">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Alias</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <StringDataCell value={users[0].id} onChange={handleChangeIdFirst} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                            <td>
                                <StringDataCell value={users[0].alias} onChange={handleChangeAliasFirst} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <StringDataCell value={users[1].id} onChange={handleChangeIdSecond} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                            <td>
                                <StringDataCell value={users[1].alias} onChange={handleChangeAliasSecond} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p>{JSON.stringify(users)}</p>
        </>
    )
}