import { Navigate, useRouter } from "@tanstack/react-router";
import { useState, useContext } from "react";
import { authenticateUser } from "../api/endpoints";
import { Authentication } from "../api/types";
import { AppContext } from "../main";
import AuthenticateForm from "./AuthenticateForm";

/* eslint-disable react/react-in-jsx-scope */

interface Props {
    authenticationType: "login" | "create"
}

export default function Authenticate({ authenticationType }: Props) {
    const [formMessage, setFormMessage] = useState<string | undefined>(undefined);
    const context = useContext(AppContext);
    const router = useRouter();

    if (context.currentUser) {
        return <Navigate to="/dashboard" search={{ user: context.currentUser.alias }} />;
    }

    const handleSubmit = async (fields: Authentication) => {
        try {
            const existingUser = await authenticateUser(fields);
            context.currentUser = existingUser;
            router.navigate({ to: "/dashboard", search: { user: existingUser.alias } });
        }
        catch (error) {
            setFormMessage((error as Error).message)
        }
    }

    const submitLabel = authenticationType == 'login'
        ? 'Login'
        : "Create Account";
    const switchPageAddress = authenticationType == 'login'
        ? '/createAccount'
        : '/login';
    const switchButtonLabel = authenticationType == 'login'
        ? 'New here?'
        : 'Already a user?';

    return (
        <div className="p-4 rounded-lg bg-slate-800 border-2" >
            <h2>Welcome!</h2>
            <AuthenticateForm
                submitLabel={submitLabel}
                onSuccess={(fields) => handleSubmit(fields)}
                errorMessage={formMessage} />
            <button onClick={() => router.navigate({ to: switchPageAddress })}>
                {switchButtonLabel}
            </button>
        </div>)
}
