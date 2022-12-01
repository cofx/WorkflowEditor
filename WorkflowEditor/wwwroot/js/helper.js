function enableTheme(theme){
  Array.from(document.querySelectorAll('link'))
  .filter(f=>f.id!="")
  .forEach(f=>{
    f.rel = f.id==theme?'stylesheet':'stylesheet';
    f.disabled = f.id!=theme;
  });
}
