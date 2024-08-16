import { useContext } from 'react';
import { GlobalContext } from '../main';
/* eslint-disable react/react-in-jsx-scope */

export default function ActiveUser() {
    const globalContext = useContext(GlobalContext);
    return (
        <div className='rounded-md border-t-yellow-300 p-4'>
            {globalContext.signedInAs &&
                <>
                    <h2>Signed in as</h2>
                    <p>{globalContext.signedInAs.alias}</p>
                    <button>Sign out</button>
                </>
            }
        </div>
    );
}