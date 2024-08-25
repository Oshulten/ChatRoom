import createClient, { Middleware } from "openapi-fetch"
import { paths } from './schema';
import { User, Authentication, Space, MessageSequence, MessagePost, Message } from "./types";

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


export async function createUser(request: Authentication) {
    const { data, response } = await client.POST("/api/Authentication/create-user", { body: { ...request } });

    if (response.ok) return data as User;

    throw new Error("Username already exists");
}

export async function authenticateUser(request: Authentication) {
    const { data, response } = await client.POST("/api/Authentication/authorize-user", { body: { ...request } });

    if (response.ok) return data as User;

    throw new Error("Username or password is invalid");
}

export async function getSpacesByUserId(userGuid: string | undefined) {
    if (!userGuid) throw Error("An id must be provided")

    const { data } = await client.GET("/api/Spaces", {
        params: {
            query: { userGuid }
        }
    });

    return data as Space[];
}

export async function getUserByUserId(userGuid: string) {
    const { data } = await client.GET("/api/Users/{userGuid}", {
        params: {
            path: { userGuid }
        }
    });

    return data as User;
}

export async function getLastMessagesInSpace(spaceGuid: string, getBeforeDate: Date, numberOfMessages: number) {
    const { data } = await client.GET("/api/Messages", {
        params: {
            query: {
                spaceGuid: spaceGuid,
                messagesBefore: getBeforeDate.toISOString(),
                numberOfMessages: numberOfMessages,
            }
        }
    });

    return data as unknown as MessageSequence;
}

export async function postMessage({ content, spaceGuid, senderGuid }: MessagePost) {
    console.log({ content, spaceId: spaceGuid, userId: senderGuid });
    const { data } = await client.POST("/api/Messages", {
        body: { content, spaceId: spaceGuid, userId: senderGuid }
    });
    return data as Message;
}