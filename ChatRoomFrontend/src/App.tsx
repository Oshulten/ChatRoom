import React from 'react'
import './App.css'
import { UserTable, GenericCell } from './components/DataTable'


function App() {
  return (
    <>
      <UserTable></UserTable>
      <GenericCell value={Boolean(true)} onChange={() => undefined} />
      <GenericCell value={true} onChange={() => undefined} />
      <GenericCell value={3.5} onChange={() => undefined} />
      <GenericCell value={"my string"} onChange={() => undefined} />
      <GenericCell value={["my string in array"]} onChange={() => undefined} />
      <GenericCell value={["my string in array"]} onChange={() => undefined} />
      <GenericCell value={{ id: 3, value: "dfsa" }} onChange={() => undefined} />
      <GenericCell value={new Date(Date.now())} onChange={() => undefined} />
    </>
  )
}

export default App
