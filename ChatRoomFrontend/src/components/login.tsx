/* eslint-disable react/react-in-jsx-scope */
import { useForm, SubmitHandler } from "react-hook-form"

interface AuthenticationInputs {
    username: string
    password: string
}

export default function Login() {
    const { register, handleSubmit, formState: { errors } } = useForm<AuthenticationInputs>({
        defaultValues: {
            username: "",
            password: ""
        }
    });
    const onSubmit: SubmitHandler<AuthenticationInputs> = (data) => console.log(data);

    console.log(errors);

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <div>
                <input
                    {...register("username", {
                        required: "Username is required",
                        minLength: { value: 3, message: "Username must be longer than 3 characters" }
                    })}
                    placeholder="Username"
                    className="input input-bordered w-full max-w-xs" />
                <p>{errors["username"] ? errors.username.message : ' '}</p>
                <input
                    type="password"
                    {...register("password", {
                        required: "Password is required",
                        minLength: { value: 5, message: "Password must be longer than 5 characters" }
                    })}
                    placeholder="Password"
                    className="input input-bordered w-full max-w-xs" />
                <p>{errors.password ? errors.password.message : 'no errors'}</p>
            </div>
            <input type="submit" />
        </form>
    )

}
