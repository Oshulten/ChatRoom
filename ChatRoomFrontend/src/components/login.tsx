import { useEffect, useRef, useState } from "react";
import { ChatUserClass } from "../types/chatUser";
import { ChatSpaceClass } from "../types/chatSpace";
import { ChatMessageClass } from "../types/chatMessage";
import { dateRepresentationRelativeToNow } from "../utilities/dateRepresentation";

type ChatRoomState = "login" | "login-failed" | "pick-space" | "in-space";

/* eslint-disable react/react-in-jsx-scope */
export function Login() {
    const [user, setUser] = useState<ChatUserClass | null>(null);
    const [roomState, setRoomState] = useState<ChatRoomState>("login");
    const [availableSpaces, setAvailableSpaces] = useState<ChatSpaceClass[] | null>(null);
    const [presentUsers, setPresentUsers] = useState<ChatUserClass[] | null>(null);
    const [messages, setMessages] = useState<ChatMessageClass[] | null>(null);
    const modalLoginRef = useRef<HTMLDialogElement>(null);
    const modalPickSpaceRef = useRef<HTMLDialogElement>(null);
    const userNameRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        switch (roomState) {
            case "login":
                if (modalLoginRef.current) {
                    modalLoginRef.current.showModal();
                }
                break;
            case "pick-space":
                if (modalPickSpaceRef.current) {
                    modalPickSpaceRef.current.showModal();
                }
                break;
        }
    }, [roomState]);

    const loginAttempt = async () => {
        if (userNameRef.current && passwordRef.current) {
            try {
                const response = await fetch("http://localhost:5055/api/ChatUsers/login", {
                    method: "POST",
                    body: JSON.stringify({
                        username: userNameRef.current.value,
                        password: passwordRef.current.value
                    }),
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (!response.ok) {
                    throw new Error(`Response status: ${response.status}`);
                }

                const user = new ChatUserClass(await response.json());
                console.log(`Logged in as ${userNameRef.current.value}`);
                setUser(user);
                setRoomState("pick-space");

                const spacesResponse = await fetch(`http://localhost:5055/api/ChatSpaces/by-user/${user.id}`);
                const spaces = (await spacesResponse.json() as object[]).map(spaceJson => new ChatSpaceClass(spaceJson));
                setAvailableSpaces(spaces);
            } catch (error) {
                console.error(`Something went wrong on the backend ${(error as Error).message}`);
                setRoomState("login-failed");
            }
        }
    }

    const pickSpace = async (spaceId: string) => {
        if (availableSpaces) {
            const chosenSpace = availableSpaces.find(space => space.id == spaceId);
            const usersResponse = await fetch(`http://localhost:5055/api/ChatUsers/get-by-space/${chosenSpace!.id}`);
            const usersInSpace = (await usersResponse.json() as object[]).map(userJson => new ChatUserClass(userJson));
            setPresentUsers(usersInSpace);
            const messagesResponse = await fetch(`http://localhost:5055/api/ChatMessages/by-space/${chosenSpace!.id}`);
            const messages = (await messagesResponse.json() as object[]).map(messageObject => new ChatMessageClass(messageObject));
            messages.sort((a, b) => (a.postedAt.getTime() - b.postedAt.getTime()));
            console.log(JSON.stringify(messages));
            setMessages(messages);
            setRoomState("in-space");
        }
    }



    switch (roomState) {
        case "login-failed":
            return <h2>Login failed</h2>

        case "login":
            return (
                <>
                    <dialog ref={modalLoginRef} id="my_modal_1" className="modal">
                        <div className="modal-box">
                            <h3 className="font-bold text-lg">Welcome to Chat Spaces!</h3>
                            <div className="modal-action place-content-center">
                                <form method="dialog">
                                    <div>
                                        <input ref={userNameRef} type="text" placeholder="User name" className="input input-bordered w-full max-w-xs" />
                                    </div>
                                    <div>
                                        <input ref={passwordRef} type="password" placeholder="Password" className="input input-bordered w-full max-w-xs" />
                                    </div>
                                    <br />
                                    <button className="btn" onClick={loginAttempt}>Login</button>
                                </form>
                            </div>
                        </div>
                    </dialog>
                </>
            );
        case "pick-space":
            return (
                <dialog ref={modalPickSpaceRef} id="my_modal_1" className="modal">
                    <div className="modal-box">
                        <h3 className="font-bold text-lg">{`Which space do you want to hang out in, ${user ? user.alias : ""}?`}</h3>
                        <div className="modal-action place-content-center">
                            <form method="dialog">
                                <select className="select w-full max-w-xs" onChange={(e) => pickSpace(e.target.value)}>
                                    <option key="0" disabled selected>Pick space</option>
                                    {availableSpaces && availableSpaces.map(space => <option key={space.id} value={space.id}>{space.alias}</option>)}
                                </select>
                            </form>
                        </div>
                    </div>
                </dialog>
            );
        case "in-space":
            return (
                <div>
                    {messages != null && messages.map(message => {
                        const relativeTime = dateRepresentationRelativeToNow(message.postedAt, new Date(Date()));
                        return (
                            <div key={message.id} className="rounded-2xl bg-gray-700 m-2 p-1">
                                <h2>{presentUsers!.find(user => user.id == message.userId)!.alias}</h2>
                                <br />
                                <p className="italic">{message.content}</p>
                                <br />
                                <p>{`Sent ${relativeTime}`}</p>
                            </div>
                        )
                    })
                    }
                </div>
            );
    }
}