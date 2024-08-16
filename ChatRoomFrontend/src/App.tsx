/* eslint-disable react/react-in-jsx-scope */
import { useState } from 'react';
import './App.css'
import AuthenticateForm, { AuthenticationFields } from './components/authenticateForm'
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { authorizeUser } from './api/authorizeUser';
import { createAccount } from './api/createAccount';

export const baseUrl = "http://localhost:5055/api";

type LoginState = "login" | "createAccount";

export default function App() {
  const [loginState, setLoginState] = useState<LoginState>("login");
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
    enabled: loginField !== null && loginState == "login",
  });

  const createAccountQuery = useQuery({
    queryKey: ["createAccount"],
    queryFn: () => createAccount(baseUrl, loginField!),
    enabled: loginField !== null && loginState == "createAccount"
  })

  if (loginQuery.isSuccess) {
    if (loginQuery.data != null) {
      console.log(`Logged in as ${loginQuery.data.alias}`)
      //navigate to space page
    } else {
      console.log(`Failed to login`);
      //stay on page, give error message
    }
  }

  if (loginState == "login") {
    return (
      <div className="p-4 rounded-lg bg-slate-800 border-2" >
        <h2>Welcome!</h2>
        <AuthenticateForm
          submitLabel='Login'
          onSuccess={(fields) => setLoginField(fields)} />
        <button onClick={() => setLoginState("createAccount")}>New here?</button>
      </div>)
  }

  if (loginState == "createAccount") {
    return (
      <div className="p-4 rounded-lg bg-slate-800 border-2" >
        <h2>Create Account</h2>
        <AuthenticateForm
          submitLabel='Create Account'
          onSuccess={(fields) => setLoginField(fields)} />
        <button onClick={() => setLoginState("login")}>Already a user?</button>
      </div>)
  }
}
