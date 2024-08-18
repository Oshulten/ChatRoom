import createClient, { Middleware } from "openapi-fetch"
import { paths } from './schema';
import { UserResponse, AuthenticationRequest } from "./types";

const logRequestResponse: Middleware = {
    async onRequest({ request, schemaPath }) {
        console.log(request);
        console.log(schemaPath);
    },
    async onResponse({ response }) {
        console.log(response);
    }
};

const client = createClient<paths>({ baseUrl: 'http://localhost:5055' });
client.use(logRequestResponse);

export async function getUsers() {
    const { data } = await client.GET("/api/ChatMessages");
    return data as UserResponse[];
}

export async function getUserById(id: string) {
    const { data, response } = await client.GET("/api/ChatMessages/{id}", { params: { path: { id } } });
    if (response.ok) return data as UserResponse;
    throw Error("Provided ID could not be found");
}

export async function createUser(request: AuthenticationRequest) {
    const { data, response } = await client.POST("/api/Authentication/create-user", { body: { ...request } });
    if (response.ok) return data as UserResponse;
    throw new Error("Username already exists");
}

export async function authenticateUser(request: AuthenticationRequest) {
    const { data, response } = await client.POST("/api/Authentication/authorize-user", { body: { ...request } });
    if (response.ok) return data as UserResponse;
    throw new Error("Username or password is invalid");
}