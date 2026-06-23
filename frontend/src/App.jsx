import { useState } from 'react';
import CustomerOnboardingForm from './components/CustomerOnboardingForm';
import Confirmation from './components/Confirmation';
import './App.css';

function App() {
  const [registeredCustomer, setRegisteredCustomer] = useState(null);

  if (registeredCustomer) {
    return (
      <div className="app">
        <Confirmation
          customer={registeredCustomer}
          onRegisterAnother={() => setRegisteredCustomer(null)}
        />
      </div>
    );
  }

  return (
    <div className="app">
      <CustomerOnboardingForm onSuccess={setRegisteredCustomer} />
    </div>
  );
}

export default App;
