import React from 'react'
import './App.css'
import { StringDataCell, lengthPattern, maxLengthPattern } from './components/DataTable'

function App() {
  return (
    <>
      <StringDataCell initialValue="123" validationPattern={lengthPattern(0, 4)}></StringDataCell>
    </>
  )
}

export default App
