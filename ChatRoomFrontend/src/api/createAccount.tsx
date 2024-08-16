import { AuthenticationFields } from "../components/authenticateForm";
import User from "../types/user";

export async function createAccount(baseUrl: string, fields: AuthenticationFields): Promise<User | undefined> {
    const response = await fetch(`${baseUrl}/ChatUsers/create-account`, {
        method: "POST",
        body: JSON.stringify(fields),
        headers: {
            "Content-Type": "application/json",
        }
    });

    if (!response.ok || response.status == 204) {
        return;
    }

    return (await response.json()) as User;
}
