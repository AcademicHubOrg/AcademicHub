import React, { useEffect, useRef } from 'react';
import Quill from 'quill';
import 'quill/dist/quill.snow.css';

interface RichTextEditorProps {
    onFileUpload: (fileContent: string) => void;
}

const RichTextEditor: React.FC<RichTextEditorProps> = ({ onFileUpload }) => {
    const quillRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const quill = new Quill(quillRef.current as HTMLElement, {
            theme: 'snow',
            placeholder: 'Write something...',
        });

        // Optional: Add event listeners or customize Quill further if needed

        return () => {
            quill.off('text-change', () => {});
            quill.off('selection-change', () => {});
        };
    }, []);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];

        if (file) {
            const reader = new FileReader();
            reader.onload = (e) => {
                const fileContent = e.target?.result as string;
                onFileUpload(fileContent);
            };
            reader.readAsText(file);
        }
    };

    return (
        <div>
            <div ref={quillRef} />
            <input type="file" onChange={handleFileChange} />
        </div>
    );
};

export default RichTextEditor;
