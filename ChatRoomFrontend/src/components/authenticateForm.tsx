/* eslint-disable react/react-in-jsx-scope */
import { useForm, SubmitHandler } from "react-hook-form"

interface AuthenticationFields {
    username: string
    password: string
}

interface AutenticateFormProps {
    onSuccess: (fields: AuthenticationFields) => void
    submitLabel: string
}

export default function AuthenticateForm({ onSuccess, submitLabel }: AutenticateFormProps) {
    const form = useForm<AuthenticationFields>({
        defaultValues: {
            username: "",
            password: ""
        }
    });

    const onSubmit: SubmitHandler<AuthenticationFields> = (fields) => onSuccess(fields);

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <div>
                <input
                    {...form.register("username", {
                        required: "Username is required",
                        minLength: { value: 3, message: "Username must be longer than 3 characters" }
                    })}
                    placeholder="Username"
                    className="input input-bordered w-full max-w-xs" />
                <input
                    type="password"
                    {...form.register("password", {
                        required: "Password is required",
                        minLength: { value: 5, message: "Password must be longer than 5 characters" }
                    })}
                    placeholder="Password"
                    className="input input-bordered w-full max-w-xs" />
            </div>
            <button
                type="submit"
                disabled={!form.formState.isValid}
                className="btn btn-neutral">{submitLabel}</button>
        </form>
    )

}
