/* eslint-disable react/react-in-jsx-scope */
import { useState } from 'react';
import './App.css'
import { InteractiveDataCellSupportedTypes } from './components/dataTable2';
import ObjectInspector from './components/objectInspector';
import { ChatUserClass } from './types/chatUser';
import { ChatMessageClass } from './types/chatMessage';
import { ChatSpaceClass } from './types/chatSpace';

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server
//Todo: object loses it's class identity after reassembly

export default function App() {
  const [user, setUser] = useState<ChatUserClass>(ChatUserClass.fromProperties("Mike", "Mike", "myPassowrd", new Date("2024-08-11T13:00:00.000+02:00"), false));
  const [message, setMessage] = useState<ChatMessageClass>(ChatMessageClass.fromProperties("MikeMessage", "Mike", new Date("2024-08-11T13:00:00.000+02:00"), "Hello no-one!", "global"));
  const [space, setSpace] = useState<ChatSpaceClass>(ChatSpaceClass.fromProperties("global", "Global", ["Mike"]));

  const handleChangeUser = (newObject: InteractiveDataCellSupportedTypes, key: string | undefined) => {
    console.log(`onChange in App from ObjectInspector - '${key}': ${JSON.stringify(newObject)}`)
    setUser(newObject as ChatUserClass);
  }
  const handleChangeMessage = (newObject: InteractiveDataCellSupportedTypes, key: string | undefined) => {
    console.log(`onChange in App from ObjectInspector - '${key}': ${JSON.stringify(newObject)}`)
    setMessage(newObject as ChatMessageClass);
  }
  const handleChangeSpace = (newObject: InteractiveDataCellSupportedTypes, key: string | undefined) => {
    console.log(`onChange in App from ObjectInspector - '${key}': ${JSON.stringify(newObject)}`)
    setSpace(newObject as ChatSpaceClass);
  }

  return (
    <>
      <ObjectInspector subject={user} subjectKey="User" onChange={(newObject, key) => handleChangeUser(newObject, key)} />
      <ObjectInspector subject={message} subjectKey="Message" onChange={(newObject, key) => handleChangeMessage(newObject, key)} />
      <ObjectInspector subject={space} subjectKey="Space" onChange={(newObject, key) => handleChangeSpace(newObject, key)} />
      <p>{JSON.stringify([user, message, space])}</p>
    </>
  )
}
