import React from 'react'
import './App.css'
import StringDataCell, { characterLengthPattern } from './components/DataTable'

function App() {
  return (
    <>
      <StringDataCell initialValue="123" validationPattern={characterLengthPattern(2, 4)}></StringDataCell>
    </>
  )
}

export default App
