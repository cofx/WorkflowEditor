function enableTheme(theme){
  Array.from(document.querySelectorAll('link'))
  .filter(f=>f.id!="")
  .forEach(f=>{
    f.rel = f.id==theme?'stylesheet':'stylesheet';
    f.disabled = f.id!=theme;
  });
}

downloadFileFromStream = async (fileName, contentStreamReference) => {
  const arrayBuffer = await contentStreamReference.arrayBuffer();
  const blob = new Blob([arrayBuffer]);
  const url = URL.createObjectURL(blob);
  const anchorElement = document.createElement('a');
  anchorElement.href = url;
  anchorElement.download = fileName ?? '';
  anchorElement.click();
  anchorElement.remove();
  URL.revokeObjectURL(url);
}