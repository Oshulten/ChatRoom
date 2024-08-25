/* eslint-disable react/react-in-jsx-scope */
import { useContext } from "react";
import { AppContext } from "../main";

export default function DisplayContext() {
    const context = useContext(AppContext);

    return (
        <div className="border-solid">
            {Object.entries(context).map(([key, value]) => <p key={key}>{`${key}: ${JSON.stringify(value)}`}</p>)}
        </div>
    );
}