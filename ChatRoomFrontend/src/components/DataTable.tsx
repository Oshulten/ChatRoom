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
    disabled?: boolean
}

export function StringDataCell({ value, onChange, validationPattern, validationError, placeholder, disabled }: StringDataCellProps) {
    const [inputIsValid, setInputIsValid] = useState(validateWithPattern(value, validationPattern));

    validationError ??= "(Sorry, something is wrong with the input!)";
    placeholder ??= "Enter text...";
    disabled ??= false;

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
        disabled={disabled}
        className={`input input-bordered w-full max-w-xs ${!inputIsValid ? "input-error" : "input-success"}`}>
    </ input >

    return inputElement;
}

interface UserRowProps {
    user: User,
    handleChangeId: (newValue: string) => void,
    handleChangeAlias: (newValue: string) => void,
    handleChangePassword: (newValue: string) => void,
}

export function UserRow({ user, handleChangeId, handleChangeAlias, handleChangePassword }: UserRowProps) {
    return (
        <tr key={user.id}>
            <td key={"id"}>
                <StringDataCell value={user.id} disabled={true} onChange={handleChangeId}></StringDataCell>
            </td>
            <td key={"alias"}>
                <StringDataCell value={user.alias} onChange={handleChangeAlias}></StringDataCell>
            </td>
            <td key={"password"}>
                <StringDataCell value={user.password} onChange={handleChangePassword}></StringDataCell>
            </td>
        </tr>
    );
}

export function UserTable() {
    const [users, setUsers] = useState<User[]>([{
        id: "1",
        alias: "Adam",
        password: "apa",
    },
    {
        id: "2",
        alias: "Bertha",
        password: "***-",
    }
    ]);

    const reconstructUser = (user: User, key: string, value: string): User | undefined => {
        switch (key) {
            case "id": return { ...user, id: value };
            case "alias": return { ...user, alias: value };
            case "password": return { ...user, password: value };
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

    if (users.length == 0) {
        return <h2>No users, sorry!</h2>
    }

    return (
        <>
            <div className="overflow-x-auto">
                <table className="table table-xs">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Alias</th>
                            <th>Password</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => {
                            return <UserRow
                                key={user.id}
                                user={user}
                                handleChangeId={handleChange("id", users[index].id)}
                                handleChangeAlias={handleChange("alias", users[index].id)}
                                handleChangePassword={handleChange("password", users[index].id)}></UserRow>
                        })}
                    </tbody>
                </table>
            </div>
            <p>{JSON.stringify(users)}</p>
        </>
    )
}