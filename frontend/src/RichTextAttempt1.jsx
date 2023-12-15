import React from 'react';
/*import { HtmlToReact } from 'html-to-react';*/

const [editorState, setEditorState] = useState(() =>
    EditorState.createEmpty()
);

const updateTextDescription = async (state) => {
    await setEditorState(state);
    const data = convertToRaw(editorState.getCurrentContent());
};


<Editor
    editorState={editorState}
    toolbarClassName="toolbarClassName"
    wrapperClassName="wrapperClassName"
    editorClassName="editorClassName"
    onEditorStateChange={updateTextDescription}
/>


setEditorState(EditorState.createWithContent(convertFromRaw(JSON.parse(current.description))));
