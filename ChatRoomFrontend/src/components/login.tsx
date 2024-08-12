import { useState } from "react";
import { ChatUserClass } from "../types/chatUser";
import { SubmitHandler, useForm } from "react-hook-form";

/* eslint-disable react/react-in-jsx-scope */

type LoginStates = "awaiting-authentication" | "fetching-authentication" | "server-failure";

interface LoginProps {
    endpointUrl: string,
    authenticationSuccessful: (user: ChatUserClass) => void;
}

type LoginInputs = {
    alias: string,
    password: string
}

export default function Login({ endpointUrl, authenticationSuccessful }: LoginProps) {
    const [state, setState] = useState<LoginStates>("awaiting-authentication");

    const {
        register,
        handleSubmit,
        setValue,
    } = useForm<LoginInputs>();

    const onSubmit: SubmitHandler<LoginInputs> = (data) => {
        console.log(data);
        attemptAuthentication(data.alias, data.password)
    }

    const attemptAuthentication = async (alias: string, password: string) => {
        try {
            setState("fetching-authentication");
            console.log(`Fetching authentication on [${alias}, ${password}]`);
            const loginResponse = await fetch(endpointUrl, {
                method: "POST",
                body: JSON.stringify({
                    username: alias,
                    password: password
                }),
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json",
                },
            });
            console.log(loginResponse);
            switch (loginResponse.status) {
                case 200: {
                    const user = new ChatUserClass(await loginResponse.json());
                    authenticationSuccessful(user);
                    break;
                }

                case 204:
                    setState("awaiting-authentication");
                    setValue("password", "");
                    break;

                default:
                    throw new Error(`${loginResponse.statusText}`);
            }
        } catch (error) {
            console.error(`Something went wrong on the backend ${(error as Error).message}`);
            setState("server-failure");
        }
    }

    switch (state) {
        case "awaiting-authentication":
            return (
                <div>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <input
                            {...register("alias", { required: true })}
                            type="text"
                            placeholder="Username"
                            className="input input-bordered w-full max-w-xs" />
                        <br />
                        <input
                            {...register("password", { required: true })}
                            type="password"
                            placeholder="Password"
                            className="input input-bordered w-full max-w-xs" />
                        <br />
                        <button type="submit" className="btn">Login</button>
                    </form>
                </div>
            );

        case "fetching-authentication":
            return (<>
                <span className="loading loading-dots loading-lg"></span>
                <h2>Checking username and password...</h2>
            </>);

        case "server-failure":
            return (<>
                <p>❌</p>
                <h2>{`Server failure (sorry!)`}</h2>
            </>);
    }

}