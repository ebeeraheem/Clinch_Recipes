import {
	ClassicEditor,
	AccessibilityHelp,
	Alignment,
	Autoformat,
	AutoImage,
	AutoLink,
	Autosave,
	Bold,
	Code,
	CodeBlock,
	Essentials,
	FindAndReplace,
	GeneralHtmlSupport,
	Heading,
	HtmlComment,
	HtmlEmbed,
	ImageBlock,
	ImageCaption,
	ImageInsertViaUrl,
	ImageResize,
	ImageStyle,
	ImageTextAlternative,
	ImageToolbar,
	Italic,
	Link,
	LinkImage,
	List,
	Markdown,
	Paragraph,
	SelectAll,
	SourceEditing,
	SpecialCharacters,
	SpecialCharactersArrows,
	SpecialCharactersCurrency,
	SpecialCharactersEssentials,
	SpecialCharactersLatin,
	SpecialCharactersMathematical,
	SpecialCharactersText,
	Table,
	TableToolbar,
	TextTransformation,
	//Title,
	TodoList,
	Undo
} from 'ckeditor5';

const editorConfig = {
	toolbar: {
		items: [
			'undo',
			'redo',
			'|',
			'sourceEditing',
			'findAndReplace',
			'selectAll',
			'|',
			'heading',
			'|',
			'bold',
			'italic',
			'code',
			'|',
			'specialCharacters',
			'link',
			'insertImageViaUrl',
			'insertTable',
			'codeBlock',
			'htmlEmbed',
			'|',
			'alignment',
			'|',
			'bulletedList',
			'numberedList',
			'todoList',
			'|',
			'accessibilityHelp'
		],
		shouldNotGroupWhenFull: false
	},
	plugins: [
		AccessibilityHelp,
		Alignment,
		Autoformat,
		AutoImage,
		AutoLink,
		Autosave,
		Bold,
		Code,
		CodeBlock,
		Essentials,
		FindAndReplace,
		GeneralHtmlSupport,
		Heading,
		HtmlComment,
		HtmlEmbed,
		ImageBlock,
		ImageCaption,
		ImageInsertViaUrl,
		ImageResize,
		ImageStyle,
		ImageTextAlternative,
		ImageToolbar,
		Italic,
		Link,
		LinkImage,
		List,
		Markdown,
		Paragraph,
		SelectAll,
		SourceEditing,
		SpecialCharacters,
		SpecialCharactersArrows,
		SpecialCharactersCurrency,
		SpecialCharactersEssentials,
		SpecialCharactersLatin,
		SpecialCharactersMathematical,
		SpecialCharactersText,
		Table,
		TableToolbar,
		TextTransformation,
		//Title,
		TodoList,
		Undo
	],
	heading: {
		options: [
			{
				model: 'paragraph',
				title: 'Paragraph',
				class: 'ck-heading_paragraph'
			},
			{
				model: 'heading1',
				view: 'h1',
				title: 'Heading 1',
				class: 'ck-heading_heading1'
			},
			{
				model: 'heading2',
				view: 'h2',
				title: 'Heading 2',
				class: 'ck-heading_heading2'
			},
			{
				model: 'heading3',
				view: 'h3',
				title: 'Heading 3',
				class: 'ck-heading_heading3'
			},
			{
				model: 'heading4',
				view: 'h4',
				title: 'Heading 4',
				class: 'ck-heading_heading4'
			},
			{
				model: 'heading5',
				view: 'h5',
				title: 'Heading 5',
				class: 'ck-heading_heading5'
			},
			{
				model: 'heading6',
				view: 'h6',
				title: 'Heading 6',
				class: 'ck-heading_heading6'
			}
		]
	},
	htmlSupport: {
		allow: [
			{
				name: /^.*$/,
				styles: true,
				attributes: true,
				classes: true
			}
		]
	},
	image: {
		toolbar: [
			'toggleImageCaption',
			'imageTextAlternative',
			'|',
			'imageStyle:alignBlockLeft',
			'imageStyle:block',
			'imageStyle:alignBlockRight',
			'|',
			'resizeImage'
		],
		styles: {
			options: ['alignBlockLeft', 'block', 'alignBlockRight']
		}
	},
	initialData:
		'',
	link: {
		addTargetToExternalLinks: true,
		defaultProtocol: 'https://',
		decorators: {
			toggleDownloadable: {
				mode: 'manual',
				label: 'Downloadable',
				attributes: {
					download: 'file'
				}
			}
		}
	},
	placeholder: 'Type or paste your content here!',
	table: {
		contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells']
	}
};

// The original unmodified editor
// ClassicEditor.create(document.querySelector('#editor'), editorConfig);

// Add client-side validation to editor content
let editorInstance;

ClassicEditor
	.create(document.querySelector('#editor'), editorConfig)
	.then(editor => {
		editorInstance = editor;

		// Add event listener for content changes in CKEditor
		editor.model.document.on('change:data', () => {
			// Remove any previous error message when content changes
			const existingErrorMessage = document.querySelector('.editor-container__editor .text-danger');
			if (existingErrorMessage) {
				existingErrorMessage.remove();
			}
		});
	})
	.catch(error => {
		console.error('Error initializing CKEditor:', error);
	});

const formContent = document.querySelector("#editorForm");

formContent.addEventListener("submit", function (event) {
	const editorContent = editorInstance.getData().trim();

	// Remove any previous error message
	const existingErrorMessage = document.querySelector('.editor-container__editor .text-danger');
	if (existingErrorMessage) {
		existingErrorMessage.remove();
	}

	// Check if editor content is empty
	if (editorContent === '' || editorContent === '<p class="ck-placeholder" data-placeholder="Type or paste your content here!"><br data-cke-filler="true"></p>') {
		event.preventDefault(); // Prevent form submission

		// Create and append error message
		const errorMessage = document.createElement('span');
		errorMessage.className = 'text-danger';
		errorMessage.textContent = 'The Content field is required.';
		document.querySelector('.editor-container__editor').appendChild(errorMessage);
	}
});
