import React, { useState } from 'react'
import { lengthPattern } from '../utilities/regexPatterns';
import User from '../models/User';
import { prettyJson } from '../utilities/prettyJson';

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

    const reconstructUser = (user: User, key: string, value: string): User | undefined => {
        switch (key) {
            case "id": return { ...user, id: value };
            case "alias": return { ...user, alias: value };
        }
    }

    const handleChange = (key: string, id: string) => {
        return (newValue: string) => {
            const user = users.find(user => user.id == id);
            if (user == undefined) {
                throw new Error(`User id ${id} cannot be found in state`);
            }
            const userIndex = users.indexOf(user);
            const newUser = reconstructUser(user, key, newValue);
            const newUsers = [...users.slice(0, userIndex), newUser!, ...users.slice(userIndex + 1)];
            setUsers(newUsers);
        };
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
                                <StringDataCell value={users[0].id} onChange={handleChange("id", users[0].id)} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                            <td>
                                <StringDataCell value={users[0].alias} onChange={handleChange("alias", users[0].id)} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <StringDataCell value={users[1].id} onChange={handleChange("id", users[1].id)} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                            <td>
                                <StringDataCell value={users[1].alias} onChange={handleChange("alias", users[1].id)} validationPattern={lengthPattern(0, 4)}></StringDataCell>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p>{JSON.stringify(users)}</p>
        </>
    )
}