import { useState } from 'react';
import SignaturePad from './SignaturePad';
import { createCustomer } from '../services/api';

const initialForm = {
  firstName: '',
  lastName: '',
  email: '',
  phoneNumber: '',
};

export default function CustomerOnboardingForm({ onSuccess }) {
  const [form, setForm] = useState(initialForm);
  const [signature, setSignature] = useState(null);
  const [errors, setErrors] = useState({});
  const [submitError, setSubmitError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const validate = () => {
    const nextErrors = {};

    if (!form.firstName.trim()) nextErrors.firstName = 'First name is required.';
    if (!form.lastName.trim()) nextErrors.lastName = 'Last name is required.';
    if (!form.email.trim()) {
      nextErrors.email = 'Email is required.';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
      nextErrors.email = 'Enter a valid email address.';
    }
    if (!form.phoneNumber.trim()) {
      nextErrors.phoneNumber = 'Phone number is required.';
    } else if (form.phoneNumber.replace(/\D/g, '').length < 7) {
      nextErrors.phoneNumber = 'Phone number must be at least 7 digits.';
    }

    setErrors(nextErrors);
    return Object.keys(nextErrors).length === 0;
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    setErrors((prev) => ({ ...prev, [name]: '' }));
    setSubmitError('');
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!validate()) return;

    setIsSubmitting(true);
    setSubmitError('');

    try {
      const customer = await createCustomer({
        firstName: form.firstName.trim(),
        lastName: form.lastName.trim(),
        email: form.email.trim(),
        phoneNumber: form.phoneNumber.trim(),
        signatureBase64: signature,
      });
      onSuccess(customer);
    } catch (error) {
      setSubmitError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form className="onboarding-form" onSubmit={handleSubmit} noValidate>
      <h1>Customer Onboarding</h1>
      <p className="subtitle">Register a new customer and capture their signature.</p>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="firstName">First Name</label>
          <input
            id="firstName"
            name="firstName"
            type="text"
            value={form.firstName}
            onChange={handleChange}
            placeholder="Carlo"
          />
          {errors.firstName && <span className="error">{errors.firstName}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="lastName">Last Name</label>
          <input
            id="lastName"
            name="lastName"
            type="text"
            value={form.lastName}
            onChange={handleChange}
            placeholder="Caliboso"
          />
          {errors.lastName && <span className="error">{errors.lastName}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="email">Email</label>
        <input
          id="email"
          name="email"
          type="email"
          value={form.email}
          onChange={handleChange}
          placeholder="carlo.caliboso@email.com"
        />
        {errors.email && <span className="error">{errors.email}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="phoneNumber">Phone Number</label>
        <input
          id="phoneNumber"
          name="phoneNumber"
          type="tel"
          value={form.phoneNumber}
          onChange={handleChange}
          placeholder="+63 917 123 4567"
        />
        {errors.phoneNumber && <span className="error">{errors.phoneNumber}</span>}
      </div>

      <SignaturePad onSignatureChange={setSignature} />

      {submitError && <div className="submit-error">{submitError}</div>}

      <button type="submit" className="btn-primary" disabled={isSubmitting}>
        {isSubmitting ? 'Registering...' : 'Register Customer'}
      </button>
    </form>
  );
}
