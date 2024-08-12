/* eslint-disable react/react-in-jsx-scope */
import { useEffect, useState } from "react";
import ComplexClass from "../../types/complexClass";
import ObjectInspector from "../objectInspector";
import { ChatUserClass } from "../../types/chatUser";
import { ChatMessageClass } from "../../types/chatMessage";
import { ChatSpaceClass } from "../../types/chatSpace";

const baseUrl = "http://localhost:5055/api";

export default function ObjectInspectorLayout() {
    const [complexEntity, setComplexEntity] = useState<ComplexClass>();
    const [userEntity, setUserEntity] = useState<ChatUserClass>();
    const [messageEntity, setMessageEntity] = useState<ChatMessageClass>();
    const [spaceEntity, setSpaceEntity] = useState<ChatSpaceClass>();

    useEffect(() => {
        async function getNow() {
            let response = await fetch(`${baseUrl}/Playground/complex-class`);
            const complexClass = new ComplexClass(await response.json());
            setComplexEntity(complexClass);

            response = await fetch(`${baseUrl}/ChatUsers/get-first`);
            const user = new ChatUserClass(await response.json());
            setUserEntity(user);

            response = await fetch(`${baseUrl}/ChatMessages/get-first`);
            const message = new ChatMessageClass(await response.json());
            setMessageEntity(message);

            response = await fetch(`${baseUrl}/ChatSpaces/get-first`);
            const space = new ChatSpaceClass(await response.json());
            setSpaceEntity(space);
        }
        getNow();
    }, []);

    if (complexEntity && messageEntity && spaceEntity && userEntity) {
        return (
            <>
                <div className="flex-col p-4">
                    <ObjectInspector
                        subject={
                            [
                                false,
                                "blue",
                                3,
                                ChatUserClass.fromProperties("MockId_126126", "McGregor", "1337", new Date(), true),
                                (argument: string) => console.log(argument)
                            ]}
                        subjectKey="Literal array with mixed types"
                        onChange={(newObject) => console.log(newObject)} />

                    <br />
                    <ObjectInspector
                        subject={userEntity}
                        subjectKey="Fetched User"
                        onChange={(newObject) => setUserEntity(newObject as ChatUserClass)} />
                    <br />
                    <ObjectInspector
                        subject={spaceEntity}
                        subjectKey="Fetched Space"
                        onChange={(newObject) => setSpaceEntity(newObject as ChatSpaceClass)} />
                    <br />
                    <ObjectInspector
                        subject={messageEntity}
                        subjectKey="Fetched Message"
                        onChange={(newObject) => setMessageEntity(newObject as ChatMessageClass)} />
                    <br />
                    <ObjectInspector
                        subject={complexEntity}
                        subjectKey="Fetched Complex Entity"
                        onChange={(newObject) => setComplexEntity(newObject as ComplexClass)} />
                    <br />
                </div>
            </>
        )
    }
    else return <><span className="loading loading-spinner loading-lg"></span></>
}