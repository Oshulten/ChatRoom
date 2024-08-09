import React, { useEffect, useState } from 'react'
import User from '../models/User';
import { castStringToPrimitive } from '../utilities/casting';
import type { PrimitiveType } from '../utilities/casting';
import { preProcessFile } from 'typescript';

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

interface GenericCellProps {
    value: PrimitiveType,
    onChange: (value: PrimitiveType) => void
}

export function PrimitiveDataCell({ value, onChange }: GenericCellProps) {
    function handleChange(currentString: string) {
        console.log("handleChangeInCell: " + currentString);
        onChange(castStringToPrimitive(currentString, value));
    }

    let inputElement;
    if (typeof (value) === "string") {
        inputElement =
            <input
                type="text"
                className={`input input-bordered w-full max-w-xs`}
                value={value.toString()}
                onChange={(e) => handleChange(e.target.value)} />
    }
    if (typeof (value) === "boolean") {
        inputElement =
            <select
                className="select select-bordered w-full max-w-xs"
                onChange={(e) => handleChange(e.target.value)}
                defaultValue={value.toString()}>
                <option value="true">True</option>
                <option value="false">False</option>
            </select>
    }
    if (typeof (value) === "number") {
        inputElement =
            <input
                type="number"
                className={`bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500`}
                value={value.toString()}
                onChange={(e) => handleChange(e.target.value)} />
    }

    return (<>
        {inputElement}
        {/* <p>{castToTypeInfo(value)}</p> */}
        {/* <p>{typeof (castStringToPrimitive(String(value), value))}</p> */}
    </>)
}

interface Identifiable {
    id: string
}

interface SampleEntity extends Identifiable {
    numberValue: number,
    booleanValue: boolean,
    stringValue: string
}


export function PrimitiveDataRow<T extends Identifiable>() {
    const [entity, setEntity] = useState<SampleEntity>({
        id: "3153",
        numberValue: 125,
        booleanValue: true,
        stringValue: "monkey"
    });

    function handleChangeFactory(propertyKey: keyof SampleEntity) {
        type BlankSlate = {
            [key: string]: PrimitiveType
        }
        return (newPropertyValue: PrimitiveType) => {
            const filledSlate = Object.entries({ ...entity }).reduce((accumulator, [key, value]) => {
                if (propertyKey == key) {
                    accumulator[key] = newPropertyValue;
                    return accumulator;
                }
                accumulator[key] = value;
                return accumulator;
            }, {} as BlankSlate);
            const newEntity = (filledSlate as unknown) as SampleEntity;
            setEntity(newEntity);
        }
    }

    return (
        <div className="overflow-x-auto">
            <table className="table table-xs">
                <thead>
                    <tr>
                        {Object.keys(entity).map((name) => (<th key={name}>{name}</th>))}
                    </tr>
                </thead>
                <tbody>
                    <tr key={entity.id}>
                        <td key={"id"}>
                            <PrimitiveDataCell value={entity.id} onChange={handleChangeFactory("id")}></PrimitiveDataCell>
                        </td>
                        <td key={"alias"}>
                            <PrimitiveDataCell value={entity.numberValue} onChange={handleChangeFactory("numberValue")}></PrimitiveDataCell>
                        </td>
                        <td key={"password"}>
                            <PrimitiveDataCell value={entity.booleanValue} onChange={handleChangeFactory("booleanValue")}></PrimitiveDataCell>
                        </td>
                        <td key={"string"}>
                            <PrimitiveDataCell value={entity.stringValue} onChange={handleChangeFactory("stringValue")}></PrimitiveDataCell>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
}



interface UserRowProps {
    user: User,
    handleChanges: ((newValue: string) => void)[]
}

export function UserRow({ user, handleChanges }: UserRowProps) {
    return (
        <tr key={user.id}>
            <td key={"id"}>
                <StringDataCell value={user.id} disabled={true} onChange={handleChanges[0]}></StringDataCell>
            </td>
            <td key={"alias"}>
                <StringDataCell value={user.alias} onChange={handleChanges[1]}></StringDataCell>
            </td>
            <td key={"password"}>
                <StringDataCell value={user.password} onChange={handleChanges[2]}></StringDataCell>
            </td>
        </tr>
    );
}

export function UserTable() {
    const [users, setUsers] = useState<User[]>([]);
    // const [users, setUsers] = useState<User[]>([{
    //     id: "1",
    //     alias: "Adam",
    //     password: "apa",
    // },
    // {
    //     id: "2",
    //     alias: "Bertha",
    //     password: "***-",
    // }
    // ]);

    useEffect(() => {
        const fetchAllUsers = async () => {
            const url = "http://localhost:5055/api/Users";
            try {
                const response = await fetch(url);
                if (!response.ok) {
                    throw new Error(`Response status: ${response.status}`);
                }

                const json = await response.json() as User[];
                setUsers(json);
            } catch (error) {
                console.error((error as Error).message);
            }
        }
        fetchAllUsers();
    }, []);

    useEffect(() => {
        const patchUser = async (user: User) => {
            const url = `http://localhost:5055/api/Users/${user.id}`;
            try {
                const response = await fetch(url, {
                    method: "PATCH",
                    body: JSON.stringify(user),
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (!response.ok) {
                    throw new Error(`Response status: ${response.status}`);
                }
            } catch (error) {
                console.error((error as Error).message);
            }
        }
        users.forEach((user) => {
            patchUser(user);
        })
    }, [users]);

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
                            {Object.keys(users[0]).map((name) => (<th key={name}>{name}</th>))}
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => {
                            const userEntries = Object.entries(user);
                            return (<UserRow
                                key={user.id}
                                user={user}
                                handleChanges={userEntries.map(([key]) => handleChange(key, users[index].id))}>
                            </UserRow>)
                        })
                        }
                    </tbody>
                </table>
            </div>
        </>
    )
}