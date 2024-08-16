/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'
import { useState } from 'react';
import AuthenticateForm, { AuthenticationFields } from '../components/authenticateForm';
import { useQuery } from '@tanstack/react-query';
import { authorizeUser } from '../api/authorizeUser';

export const baseUrl = "http://localhost:5055/api";

export const Route = createFileRoute('/login')({
  component: () => <Login />
})

function Login() {
  const [loginField, setLoginField] = useState<AuthenticationFields | null>(null);

  const loginQuery = useQuery({
    queryKey: ["login"],
    queryFn: async () => {
      const user = await authorizeUser(baseUrl, loginField!);
      if (user === null) {
        setLoginField(null);
      }
      return user;
    },
    enabled: loginField !== null,
  });

  if (loginQuery.isSuccess) {
    if (loginQuery.data != null) {
      console.log(`Logged in as ${loginQuery.data.alias}`)
      //navigate to space page
    } else {
      console.log(`Failed to login`);
      //stay on page, give error message
    }
  }

  return (
    <div className="p-4 rounded-lg bg-slate-800 border-2" >
      <h2>Welcome!</h2>
      <AuthenticateForm
        submitLabel='Login'
        onSuccess={(fields) => setLoginField(fields)} />
      <button onClick={() => undefined}>New here?</button>
    </div>)
}
