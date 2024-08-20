/* eslint-disable react/react-in-jsx-scope */
import { useForm, SubmitHandler } from "react-hook-form"
import { Authentication } from "../api/types";


interface Props {
    onSuccess: (fields: Authentication) => void
    submitLabel: string
    errorMessage?: string
}

export default function AuthenticateForm({ onSuccess, submitLabel, errorMessage }: Props) {
    const form = useForm<Authentication>({
        defaultValues: {
            username: "",
            password: ""
        }
    });

    const onSubmit: SubmitHandler<Authentication> = (fields) => onSuccess(fields);

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <div className="p-4">
                <input
                    {...form.register("username", {
                        required: "Username is required",
                        minLength: { value: 3, message: "Username must be longer than 3 characters" }
                    })}
                    placeholder="Username"
                    className="input input-bordered w-full max-w-xs m-1 " />
                <input
                    type="password"
                    {...form.register("password", {
                        required: "Password is required",
                        minLength: { value: 3, message: "Password must be longer than 3 characters" }
                    })}
                    placeholder="Password"
                    className="input input-bordered w-full max-w-xs m-1" />
            </div>
            {errorMessage && <p className="m-1">{errorMessage}</p>}
            <button
                type="submit"
                disabled={!form.formState.isValid}
                className="btn btn-neutral m-2">{submitLabel}</button>
        </form>
    )

}
