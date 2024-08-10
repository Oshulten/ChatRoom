import React, { useEffect, useState } from 'react'
import './App.css'
import { ChatUserClass } from './types/chatUser';
import ObjectInspector from './components/objectInspector';
import { ComplexClass } from './types/complexClass';
// import PrimitiveDataTable from './components/dataTable'
// import ChatUser from './types/chatUser'
// import ChatMessage from './types/chatMessage'
// import ChatSpace from './types/chatSpace'

const baseUrl = "http://localhost:5055/api";

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server

function App() {
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
      <>
        <ObjectInspector subject={[1, 2, 3, 4]} subjectKey="literal array" />
        <br />
        <ObjectInspector subject={
          [
            false,
            "blue",
            3,
            ChatUserClass.fromProperties("McGregor", "1337", new Date(), true),
            (argument: string) => console.log(argument)
          ]} subjectKey="literal array with mixed types" />
        <br />
        <ObjectInspector subject={entity} subjectKey="fetched entity" />

      </>
    )
  }
  return <p>Fetching...</p>
}

export default App
