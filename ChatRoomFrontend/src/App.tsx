import React from 'react'
import './App.css'
import StringDataCell from './components/DataTable'

function App() {
  return (
    <>
      <StringDataCell initialValue="123" validationPattern="^apa$"></StringDataCell>
    </>
  )
}

export default App
