/* eslint-disable react/react-in-jsx-scope */
import { QueryClient, QueryClientProvider, useQuery } from '@tanstack/react-query';
import './App.css'
import Login from './components/login'

const fetchAllMessages = async () => {

}

export default function App() {
  const messagesQuery = useQuery({
    queryKey: ["messages"],
    queryFn: fetchAllMessages
  });


  console.log(messagesQuery);

  return (
    <Login />
  )
}
