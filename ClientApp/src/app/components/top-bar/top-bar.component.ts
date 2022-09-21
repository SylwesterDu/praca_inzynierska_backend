import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.scss']
})
export class TopBarComponent implements OnInit {
  isSearching: boolean = false;

  popularMusicTags: string[] =[
    'rock',
    'pop',
    'rap',
    'metal',
    'blues',
    'muzyka_klasyczna',
    'techno',
  ];

  popularLiteratureTags: string[] =[
    'dramat',
    'komedia',
    'kryminał',
    'horror',
    'literatura_piękna',
    'romans',
  ];

  popularOtherTags: string[] =[
    'wzory_drukarek_3d',
    'origami',
    'wzory_cnc',
    'metal',    
  ];

  popularImagesTags: string[] =[
    'pejzaż',
    'art',
    'impresjonizm',
    'fotografia',    
  ];

  @ViewChild('mainPanel')
  mainPanel!: ElementRef<HTMLDivElement>;

  constructor(private renderer: Renderer2) {
    this.renderer.listen('window', 'click',(e:Event)=>{
      if(e.target !== this.mainPanel.nativeElement && !this.mainPanel.nativeElement.contains(<Node>e.target)){
        if(this.isSearching){
          this.isSearching = false;
          console.log("hide")
        }
      }
    });
  }
  

  ngOnInit(): void {
  }

  

  expand(){
    this.isSearching = true;
    console.log("expand");
  }

}
