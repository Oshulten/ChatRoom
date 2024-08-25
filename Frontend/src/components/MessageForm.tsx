/* eslint-disable react/react-in-jsx-scope */
import { useForm, SubmitHandler } from "react-hook-form"

interface MessageFields {
    message: string
}

interface Props {
    onSuccess: (message: string) => void
}

export default function AuthenticateForm({ onSuccess }: Props) {
    const form = useForm<MessageFields>({
        defaultValues: {
            message: "",
        }
    });

    const onSubmit: SubmitHandler<MessageFields> = (fields) => onSuccess(fields.message);

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <div className="p-4">
                <input
                    {...form.register("message", {
                        required: "A message is required",
                        minLength: { value: 1, message: "Message must be longer than 3 characters" }
                    })}
                    placeholder="What's on your mind?"
                    className="input input-bordered w-full max-w-xs m-1 " />
            </div>
            <button
                type="submit"
                disabled={!form.formState.isValid}
                className="btn btn-neutral m-2">➲</button>
        </form>
    )

}