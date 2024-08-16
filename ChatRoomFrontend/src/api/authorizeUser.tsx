import { AuthenticationFields } from "../components/authenticateForm";
import User from "../types/user";

export async function authorizeUser(baseUrl: string, fields: AuthenticationFields): Promise<User | null> {
    const response = await fetch(`${baseUrl}/ChatUsers/authenticate`, {
        method: "POST",
        body: JSON.stringify(fields),
        headers: {
            "Content-Type": "application/json",
        }
    });

    if (!response.ok) {
        throw new Error("User authorization failed");
    }

    if (response.status == 204) {
        return null;
    }

    return (await response.json()) as User;
}
