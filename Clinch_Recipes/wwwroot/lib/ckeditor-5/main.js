import {
	ClassicEditor,
	Alignment,
	Autoformat,
	AutoLink,
	Autosave,
	Bold,
	Code,
	CodeBlock,
	Essentials,
	FindAndReplace,
	GeneralHtmlSupport,
	Heading,
	HorizontalLine,
	HtmlComment,
	Italic,
	Link,
	List,
	Markdown,
	Mention,
	Paragraph,
	PasteFromOffice,
	Strikethrough,
	TextTransformation,
	Underline,
} from 'ckeditor5';

const editorConfig = {
	toolbar: {
		items: [
			'undo',
			'redo',
			'|',
			'findAndReplace',
			'|',
			'heading',
			'|',
			'bold',
			'italic',
			'underline',
			'strikethrough',
			'code',
			'|',
			'horizontalLine',
			'link',
			'codeBlock',
			'|',
			'alignment',
			'|',
			'bulletedList',
			'numberedList'
		],
		shouldNotGroupWhenFull: false
	},
	plugins: [
		Alignment,
		Autoformat,
		AutoLink,
		Autosave,
		Bold,
		Code,
		CodeBlock,
		Essentials,
		FindAndReplace,
		GeneralHtmlSupport,
		Heading,
		HorizontalLine,
		HtmlComment,
		Italic,
		Link,
		List,
		Markdown,
		Mention,
		Paragraph,
		PasteFromOffice,
		Strikethrough,
		TextTransformation,
		Underline
	],
	heading: {
		options: [
			{
				model: 'paragraph',
				title: 'Paragraph',
				class: 'ck-heading_paragraph'
			},
			{
				model: 'heading2',
				view: 'h2',
				title: 'Heading',
				class: 'ck-heading_heading2'
			},
			{
				model: 'heading3',
				view: 'h3',
				title: 'SubHeading',
				class: 'ck-heading_heading3'
			},
		]
	},
	codeBlock: {
		languages: [
			{ language: 'plaintext', label: 'Plain text' },

			// Web Frontend
			{ language: 'javascript', label: 'JavaScript' },
			{ language: 'typescript', label: 'TypeScript' },
			{ language: 'jsx', label: 'JSX' },
			{ language: 'tsx', label: 'TSX' },
			{ language: 'html', label: 'HTML' },
			{ language: 'css', label: 'CSS' },
			{ language: 'scss', label: 'SCSS' },

			// Backend/Systems
			{ language: 'python', label: 'Python' },
			{ language: 'java', label: 'Java' },
			{ language: 'csharp', label: 'C#' },
			{ language: 'php', label: 'PHP' },
			{ language: 'go', label: 'Go' },
			{ language: 'rust', label: 'Rust' },
			{ language: 'cpp', label: 'C++' },
			{ language: 'c', label: 'C' },

			// Mobile/Modern
			{ language: 'swift', label: 'Swift' },
			{ language: 'kotlin', label: 'Kotlin' },
			{ language: 'dart', label: 'Dart' },

			// Scripting/Other
			{ language: 'ruby', label: 'Ruby' },
			{ language: 'bash', label: 'Bash' },
			{ language: 'powershell', label: 'PowerShell' },

			// Data/Config
			{ language: 'sql', label: 'SQL' },
			{ language: 'json', label: 'JSON' },
			{ language: 'yaml', label: 'YAML' },
			{ language: 'xml', label: 'XML' },

			// Functional/Academic
			{ language: 'scala', label: 'Scala' },
			{ language: 'clojure', label: 'Clojure' },
			{ language: 'haskell', label: 'Haskell' },
			{ language: 'elixir', label: 'Elixir' },
			{ language: 'erlang', label: 'Erlang' },
			{ language: 'fsharp', label: 'F#' },
			{ language: 'lua', label: 'Lua' },

			// Data Science/Analytics
			{ language: 'r', label: 'R' },
			{ language: 'matlab', label: 'MATLAB' },
			{ language: 'julia', label: 'Julia' },

			// DevOps/Infrastructure
			{ language: 'dockerfile', label: 'Dockerfile' },
			{ language: 'makefile', label: 'Makefile' },
			{ language: 'nginx', label: 'Nginx' },

			//// Assembly/Systems
			//{ language: 'nasm', label: 'Assembly (NASM)' },
			//{ language: 'glsl', label: 'GLSL' },

			// Other Popular
			{ language: 'graphql', label: 'GraphQL' },
			{ language: 'toml', label: 'TOML' },
			{ language: 'ini', label: 'INI' },
			{ language: 'markdown', label: 'Markdown' }
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
	mention: {
		feeds: [
			{
				marker: '@',
				feed: [
					/* See: https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html */
				],
                minimumCharacters: 3
			}
		]
	},
	placeholder: 'Type or paste your content here!'
};

// Add client-side validation to editor content
ClassicEditor
	.create(document.querySelector('#ckeditor'), editorConfig)
	.then(editor => {
		window.CKEDITOR = editor; // Make CKEditor globally accessible

		// Sync content with hidden input
		const contentHidden = document.getElementById('contentHidden');

		// Set initial content if editing
		const initialContent = contentHidden.value;
		if (initialContent) {
			editor.setData(initialContent);
		}

		// Update hidden field and character count on change
		editor.model.document.on('change:data', () => {
			const data = editor.getData();
			contentHidden.value = data;
			updateContentCharacterCount(data);

			// Clear any existing error messages
            clearFieldError('Content');
		});

		// Initial character count
		updateContentCharacterCount(editor.getData());

		// Add keyboard shortcuts for common actions
		editor.keystrokes.set('Ctrl+Shift+C', (keyEvtData, cancel) => {
			editor.execute('codeBlock');
			cancel();
		});

		// Add helpful keyboard shortcuts tooltip
		const toolbar = editor.ui.view.toolbar.element;
		const codeBlockButton = toolbar.querySelector('[data-cke-tooltip-text*="Code block"]');
		if (codeBlockButton) {
			codeBlockButton.setAttribute('title', 'Code block (Ctrl+Shift+C)');
		}
	})
	.catch(error => {
		console.error('Error initializing CKEditor:', error);
		// Fallback to textarea if CKEditor fails
		showTextareaFallback();
	});

//function updateContentCharacterCount(content) {
//	// Strip HTML tags for character counting but preserve basic structure
//	const textContent = content
//		.replace(/<pre[^>]*><code[^>]*>/g, '') // Remove opening code tags
//		.replace(/<\/code><\/pre>/g, '\n') // Replace closing code tags with newlines
//		.replace(/<[^>]*>/g, '') // Remove other HTML tags
//		.replace(/&nbsp;/g, ' ') // Replace non-breaking spaces
//		.replace(/&lt;/g, '<') // Decode HTML entities
//		.replace(/&gt;/g, '>')
//		.replace(/&amp;/g, '&');

//	const length = textContent.length;
//	const maxLength = 8000;

//	const counter = document.getElementById('contentCount');
//	counter.textContent = `${length}/${maxLength}`;

//	if (length > maxLength * 0.9) {
//		counter.className = 'character-count danger';
//	} else if (length > maxLength * 0.75) {
//		counter.className = 'character-count warning';
//	} else {
//		counter.className = 'character-count';
//	}
//}

// Shared function to extract text content from HTML

function extractTextFromHtml(content) {
	// Create a temporary div to parse HTML content
	const tempDiv = document.createElement('div');
	tempDiv.innerHTML = content;

	// Walk through all nodes and extract text content
	function extractTextContent(node) {
		if (node.nodeType === Node.TEXT_NODE) {
			return node.textContent;
		} else if (node.nodeType === Node.ELEMENT_NODE) {
			let result = '';

			// Handle specific elements that should add spacing/structure
			const tagName = node.tagName.toLowerCase();

			// Add newlines for block elements
			if (['div', 'p', 'br', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'li'].includes(tagName)) {
				result += '\n';
			}

			// For pre/code blocks, preserve the content as-is (these are intentional code blocks)
			if (tagName === 'pre' || (tagName === 'code' && node.parentNode.tagName.toLowerCase() === 'pre')) {
				result += node.textContent;
			} else {
				// For other elements, recursively extract content
				for (let child of node.childNodes) {
					result += extractTextContent(child);
				}
			}

			// Add newlines after certain block elements
			if (['div', 'p', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'li'].includes(tagName)) {
				result += '\n';
			}

			return result;
		}
		return '';
	}

	let textContent = extractTextContent(tempDiv);

	// Clean up the extracted content
	textContent = textContent
		.replace(/&nbsp;/g, ' ') // Replace non-breaking spaces
		.replace(/&lt;/g, '<')   // Decode HTML entities
		.replace(/&gt;/g, '>')
		.replace(/&amp;/g, '&')
		.replace(/&quot;/g, '"')
		.replace(/&#39;/g, "'")
		.replace(/\n\s*\n/g, '\n') // Collapse multiple newlines
		.trim();

	return textContent;
}

window.extractTextFromHtml = extractTextFromHtml; // Expose for global use

// Updated character count function using the shared extraction logic
function updateContentCharacterCount(content) {
	console.log('Updating character count for content');

	const textContent = extractTextFromHtml(content);
	const length = textContent.length;
	const maxLength = 8000;

	const counter = document.getElementById('contentCount');
	counter.textContent = `${length}/${maxLength}`;

	if (length > maxLength * 0.9) {
		counter.className = 'character-count danger';
	} else if (length > maxLength * 0.75) {
		counter.className = 'character-count warning';
	} else {
		counter.className = 'character-count';
	}
}

function showTextareaFallback() {
	const container = document.getElementById('editor-container');
	const currentContent = document.getElementById('contentHidden').value || '';

	container.innerHTML = `
                    <textarea name="Content"
                              class="form-control code-editor"
                              placeholder="Paste your code here..."
                              id="contentTextarea"
                              style="min-height: 400px; font-family: 'Courier New', monospace;">${currentContent}</textarea>
                `;

	const textarea = document.getElementById('contentTextarea');
	const contentHidden = document.getElementById('contentHidden');

	// Sync textarea with hidden input
	textarea.addEventListener('input', () => {
		contentHidden.value = textarea.value;
		updateContentCharacterCount(textarea.value);
	});

	// Initial character count for fallback
	updateContentCharacterCount(currentContent);
}

function clearFieldError(fieldName) {
	const form = document.getElementById('editorForm');
    const field = form.querySelector(`[name="${fieldName}"]`);
	if (field) {
        field.classList.remove('error');
        const errorSpan = field.parentNode.querySelector('.field-validation-error');
		if (errorSpan) {
            errorSpan.textContent = '';
        }
    }
}

