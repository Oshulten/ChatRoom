/* eslint-disable react/react-in-jsx-scope */
import { useEffect, useState } from "react";
import ComplexClass from "../types/complexClass";
import ObjectInspector from "./objectInspector";
import { ChatUserClass } from "../types/chatUser";

const baseUrl = "http://localhost:5055/api";

export default function ObjectInspectorLayout({ visible }: { visible: boolean }) {
    const [entity, setEntity] = useState<ComplexClass>();

    useEffect(() => {
        async function getNow() {
            const response = await fetch(`${baseUrl}/Playground/complex-class`);
            const complexClass = new ComplexClass(await response.json());
            setEntity(complexClass);
        }
        getNow();
    }, []);

    if (entity) {
        return (
            <div hidden={!visible}>
                <div className="flex-col p-4">
                    <ObjectInspector subject={[1, 2, 3, 4]} subjectKey="Literal array" />
                    <br />
                    <ObjectInspector
                        subject={
                            [
                                false,
                                "blue",
                                3,
                                ChatUserClass.fromProperties("McGregor", "1337", new Date(), true),
                                (argument: string) => console.log(argument)
                            ]}
                        subjectKey="Literal array with mixed types" />

                    <br />

                    <ObjectInspector subject={entity} subjectKey="Fetched entity" />
                </div>
            </div>
        )
    }
    else return <></>
}