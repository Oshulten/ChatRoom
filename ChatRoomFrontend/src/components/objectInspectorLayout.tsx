/* eslint-disable react/react-in-jsx-scope */
import { useEffect, useState } from "react";
import ComplexClass from "../types/complexClass";
import ObjectInspector from "./objectInspector";
import ChatUser, { ChatUserClass } from "../types/chatUser";
import { ChatMessageClass } from "../types/chatMessage";
import { ChatSpaceClass } from "../types/chatSpace";
import { typeCheck } from "../utilities/typeCheck";

const baseUrl = "http://localhost:5055/api";

export default function ObjectInspectorLayout({ visible }: { visible: boolean }) {
    const [complexEntity, setComplexEntity] = useState<ComplexClass>();
    const [userEntity, setUserEntity] = useState<ChatUserClass>();
    const [messageEntity, setMessageEntity] = useState<ChatMessageClass>();
    const [spaceEntity, setSpaceEntity] = useState<ChatSpaceClass>();

    useEffect(() => {
        async function getNow() {
            let response = await fetch(`${baseUrl}/Playground/complex-class`);
            const complexClass = new ComplexClass(await response.json());
            setComplexEntity(complexClass);

            response = await fetch(`${baseUrl}/ChatUsers/4e36f637-730c-462a-bfec-b92ce1be7e36`);
            const user = new ChatUserClass(await response.json());
            setUserEntity(user);

            response = await fetch(`${baseUrl}/ChatMessages/1d16779d-f5ca-49c2-8384-55d3d8f32719`);
            const message = new ChatMessageClass(await response.json());
            console.log(typeCheck(message.postedAt));
            setMessageEntity(message);

            response = await fetch(`${baseUrl}/ChatSpaces/579ec7c0-b112-483c-af3e-8f5de50f4d6f`);
            const space = new ChatSpaceClass(await response.json());
            setSpaceEntity(space);
        }
        getNow();
    }, []);

    if (complexEntity && messageEntity && spaceEntity && userEntity) {
        return (
            <div hidden={!visible}>
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
                </div>
                <p>{JSON.stringify(userEntity)}</p>

                <p>{JSON.stringify(spaceEntity)}</p>
                <p>{JSON.stringify(messageEntity)}</p>
                {/* <p>{messageEntity.postedAt.toISOString().slice(0, -5)}</p>
                <input type="datetime-local" value={messageEntity.postedAt.toISOString().slice(0, -5)}></input> */}
            </div>

        )
    }
    else return <></>
}