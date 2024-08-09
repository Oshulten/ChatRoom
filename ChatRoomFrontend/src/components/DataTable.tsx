import React, { useEffect, useState } from 'react'
import User from '../types/user';
import { castStringToPrimitive } from '../utilities/casting';
import type { PrimitiveType } from '../utilities/casting';
import { GenericIdEntity } from '../types/genericIdEntity';
import useFetchAllEntities from '../hooks/useFetchAllEntities';

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
    disabled: boolean,
    onChange: (value: PrimitiveType) => void
}

export function PrimitiveDataCell({ value, onChange, disabled }: GenericCellProps) {
    const handleChange = (currentString: string) => onChange(castStringToPrimitive(currentString, value));

    let inputElement;

    if (typeof (value) === "string") {
        inputElement =
            <input
                type="text"
                className={`input input-bordered w-full max-w-xs`}
                value={value.toString()}
                onChange={(e) => handleChange(e.target.value)}
                disabled={disabled} />
    }

    if (typeof (value) === "boolean") {
        inputElement =
            <select
                className="select select-bordered w-full max-w-xs"
                onChange={(e) => handleChange(e.target.value)}
                defaultValue={value.toString()}
                disabled={disabled}>
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
                disabled={disabled}
                onChange={(e) => handleChange(e.target.value)} />
    }

    return inputElement;
}

interface PrimitiveDataTableProps {
    endpoint: string,
    showId: boolean
}

export function PrimitiveDataTable<T extends GenericIdEntity>({ endpoint, showId }: PrimitiveDataTableProps) {
    const [entities, setEntities, status] = useFetchAllEntities<T>(endpoint);

    function handleChangeFactory(entityId: string) {
        return (newEntity: T) => {
            const entity = entities.find(entity => entity.id == entityId);
            if (entity == undefined) {
                throw new Error(`Entity id ${entityId} cannot be found in state`);
            }
            const entityIndex = entities.indexOf(entity);
            const newEntities = [...entities.slice(0, entityIndex), newEntity!, ...entities.slice(entityIndex + 1)];
            setEntities(newEntities);
        }
    }
    switch (status) {
        case "fetching":
            return <span className="loading loading-spinner loading-lg"></span>;
        case "failure":
            return <>
                <h2 className="text-red-900 text-3xl">Server is asleep</h2>
                <p className="text-red-500">Terrible sorry! We hope you have wonderful day despite this.</p>
            </>
        case "success":
            return (<>
                <div className="overflow-x-auto">
                    <table className="table table-xs">
                        <thead>
                            <tr>
                                {Object.keys(entities[0]).map((key) => {
                                    if (!showId && key == "id") return;
                                    return (<th key={key}>{key}</th>)
                                }
                                )}
                            </tr>
                        </thead>
                        <tbody>
                            {entities.map(entity => {
                                return <PrimitiveDataRow<T> showId={showId} key={entity.id} entity={entity} handleChange={handleChangeFactory(entity.id)} />
                            })}
                        </tbody>
                    </table>
                </div>
                <p>{JSON.stringify(entities)}</p>
            </>)
    }
}

interface PrimitiveDataRowProps<T> {
    entity: T,
    showId: boolean,
    handleChange: (newValue: T) => void
}

export function PrimitiveDataRow<T extends GenericIdEntity>({ entity, showId, handleChange }: PrimitiveDataRowProps<T>) {
    function handleChangeFactory(propertyKey: keyof T) {
        type BlankSlate = {
            [key: string]: PrimitiveType
        }

        return (newPropertyValue: PrimitiveType) => {
            const filledSlate = Object.entries({ ...entity } as BlankSlate).reduce((accumulator, [key, value]) => {
                if (propertyKey == key) {
                    accumulator[key] = newPropertyValue;
                    return accumulator;
                }
                accumulator[key] = value;
                return accumulator;
            }, {} as BlankSlate);
            const newEntity = (filledSlate as unknown) as T;
            handleChange(newEntity);
        }
    }

    return (
        <tr key={entity.id}>
            {Object.keys(entity).map((key) => {
                if (key == "id" && !showId) return;
                return (
                    <td key={key}>
                        <PrimitiveDataCell key={key} value={entity[key]} onChange={handleChangeFactory(key)} disabled={key == "id"} />
                    </td>
                )
            })}
        </tr>
    )
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