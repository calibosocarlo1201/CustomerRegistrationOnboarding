import { useEffect, useRef } from 'react';

export default function SignaturePad({ onSignatureChange }) {
  const canvasRef = useRef(null);
  const isDrawingRef = useRef(false);

  useEffect(() => {
    const canvas = canvasRef.current;
    const context = canvas.getContext('2d');
    context.strokeStyle = '#1a1a2e';
    context.lineWidth = 2;
    context.lineCap = 'round';
    context.lineJoin = 'round';
  }, []);

  const getCoordinates = (event) => {
    const canvas = canvasRef.current;
    const rect = canvas.getBoundingClientRect();
    const scaleX = canvas.width / rect.width;
    const scaleY = canvas.height / rect.height;

    if (event.touches?.length) {
      return {
        x: (event.touches[0].clientX - rect.left) * scaleX,
        y: (event.touches[0].clientY - rect.top) * scaleY,
      };
    }

    return {
      x: (event.clientX - rect.left) * scaleX,
      y: (event.clientY - rect.top) * scaleY,
    };
  };

  const startDrawing = (event) => {
    event.preventDefault();
    isDrawingRef.current = true;
    const { x, y } = getCoordinates(event);
    const context = canvasRef.current.getContext('2d');
    context.beginPath();
    context.moveTo(x, y);
  };

  const draw = (event) => {
    if (!isDrawingRef.current) return;
    event.preventDefault();
    const { x, y } = getCoordinates(event);
    const context = canvasRef.current.getContext('2d');
    context.lineTo(x, y);
    context.stroke();
  };

  const stopDrawing = () => {
    if (!isDrawingRef.current) return;
    isDrawingRef.current = false;
    onSignatureChange(canvasRef.current.toDataURL('image/png'));
  };

  const clearSignature = () => {
    const canvas = canvasRef.current;
    const context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);
    onSignatureChange(null);
  };

  return (
    <div className="signature-pad">
      <label>Signature</label>
      <canvas
        ref={canvasRef}
        width={500}
        height={150}
        className="signature-canvas"
        onMouseDown={startDrawing}
        onMouseMove={draw}
        onMouseUp={stopDrawing}
        onMouseLeave={stopDrawing}
        onTouchStart={startDrawing}
        onTouchMove={draw}
        onTouchEnd={stopDrawing}
      />
      <button type="button" className="btn-secondary" onClick={clearSignature}>
        Clear Signature
      </button>
    </div>
  );
}
