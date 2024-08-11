/* eslint-disable react/react-in-jsx-scope */
import { useState } from 'react';
import './App.css'
import InteractiveDataCell from './components/dataTable2';

//Todo: No more than one table can be used at the same time
//Todo: Single tables don't work if there are no rows in that table on the server

export default function App() {
  const [number, setNumber] = useState(35.25);
  const [string, setString] = useState("monkey");
  const [boolean, setBoolean] = useState(true);
  const [date, setDate] = useState(new Date(Date.now()));
  const [array, setArray] = useState([1, 3, 4]);
  return (
    <>
      <InteractiveDataCell value={number} onChange={newValue => setNumber(newValue as number)} />
      <InteractiveDataCell value={string} onChange={newValue => setString(newValue as string)} />
      <InteractiveDataCell value={boolean} onChange={newValue => setBoolean(newValue as boolean)} />
      <InteractiveDataCell value={date} onChange={newValue => setDate(newValue as Date)} />
      <InteractiveDataCell value={array} onChange={newValue => setArray(newValue as Array<number>)} />
      <p>{JSON.stringify({ number, string, boolean, date, array })}</p>
    </>
  )
}
