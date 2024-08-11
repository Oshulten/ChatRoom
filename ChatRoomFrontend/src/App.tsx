/* eslint-disable react/react-in-jsx-scope */
import { useState } from 'react';
import './App.css'
import { InteractiveDataCellSupportedTypes } from './components/dataTable2';
import ObjectInspector from './components/objectInspector';
import { ChatUserClass } from './types/chatUser';

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server

//Todo: object loses it's class identity after reassembly

export default function App() {
  const [user, setUser] = useState(ChatUserClass.fromProperties("Mike", "myPassowrd", new Date("2024-08-11T13:00:00.000+02:00"), false));

  const handleChange = (newObject: InteractiveDataCellSupportedTypes, key: string | undefined) => {
    console.log(`onChange in App from ObjectInspector - '${key}': ${JSON.stringify(newObject)}`)
    setUser(newObject as ChatUserClass);
  }

  return (
    <>
      <ObjectInspector subject={user} subjectKey="user" onChange={(newObject, key) => handleChange(newObject, key)} />
      <p>{JSON.stringify(user)}</p>
    </>
  )
}
