# CAFU Generics

* 汎用的な Entity を定義し、取り扱うためのモジュールです。

## Requirement

* Unity 2017.1
* [CAFU Core v2](https://github.com/umm-projects/cafu_core)

## Install

```shell
npm install github:umm-projects/cafu_generics
```

## Usage

### Entity

* 作成したいデータの構造に合わせて Entity を作成する
  * Unity の Serializer の制約上、Generics を用いたクラスはシリアライズできないため、 `GenericEntity<>` などを継承したクラスを作る必要がある
* 通常のクラス用と、ScriptableObject 用の2種類がある
  * 違いは、ScriptableObject を継承しているかどうかのみ

#### `GenericEntity<TValue>`, `ScriptableObjectGenericEntity<TValue>`

* 任意の型を1つだけ内包する Entity
* 定数的な要素や共通的に用いる Prefab を配置する場合などに用いる

#### `GenericPairEntity<TKey, TValue>`, `ScriptableObjectGenericPairEntity<TKey, TValue>`

* Key Value のペアを管理するための Entity
* 基本的には疑似 Dictionary の一要素としての定義を行う
  * `class HogeSpriteEntity : GenericPairEntity<HogeType, Sprite>` などと定義することで、 HogeType の値に応じた Sprite のマップの一要素を作ることができる

#### `GenericListEntity<TValue>`, `ScriptableObjectGenericListEntity<TValue>`

* 任意の型をリストとして内包する Entity
* ランダムに定数の一覧から値を取得する要件に対してマスタデータを定義する場合などに用いる

#### `GenericEntityList<TGenericEntity>`, `ScriptableObjectGenericEntityList<TGenericEntity>`

* GenricEntity をリストとして内包する Entity
  * 紛らわしいが、値の型制約として `IGenericEntity` 型に限定している
* 定数 Entity の一覧から値を取得するなどの場合に用いる
  * 一応用意しているが、あまり使うコトは無さそう

#### `GenericPairEntityList<TGenericPairEntity>`, `ScriptableObjectGenericPairEntityList<TGenericPairEntity>`

* GenericPairEntity をリストとして内包する Entity
  * 紛らわしいが、値の型制約として `IGenericPairEntity` 型に限定している
* 疑似 Dictionary を作る場合などに用いる
  * 恐らく最も利用する頻度の高い Entity

### DataStore

#### `GenericDataStore`

* Entity を保持管理するためのクラス
* Scene 上の任意の GameObject に AddComponent し、シーン内で読み込みたい Entity を登録していく
  * GameObject 名は何でも良いが `DataStore` としておくと見通しが良い
  * `Scriptable Object Generic Entity List` というフィールドが Inspector 上に生えているので、そこに ScriptableObject を D&amp;D する
* `GetEntity<TGenericEntity>()` メソッドで Entity のインスタンスを返す

### Repository

#### `GenericRepository<TGenericEntity>`

* 型引数に渡した Entity を GenericDataStore から取得する
* 任意の UseCase で、このインスタンスを必要な Entity の数分生成し、 Model の構成要素として Entity を取得する
* `GetEntity()` メソッドで Entity のインスタンスを返す

### UseCase

#### `GenericUseCase<TGenericEntity>`

* Presenter から直接 Entity を参照したい場合に用いる
* CAFU 的には想定していない操作になるため、原則非推奨
* `GetEntity()` メソッドで Entity のインスタンスを返す

### Generator

* ScriptableObject のアセットファイルを作成するためのカスタムエディタウィンドウを提供
* メニューの Window &gt; CAFU &gt; Entity Generator からウィンドウを開く
* `ScriptableObjectGenericEntity` を継承したクラスが作成対象

<img width="407" alt="screenshot 2018-02-15 12 51 09" src="https://user-images.githubusercontent.com/838945/36239702-09d5d23a-124f-11e8-808a-17e6ff5aa698.png">
<img width="434" alt="screenshot 2018-02-15 12 51 17" src="https://user-images.githubusercontent.com/838945/36239703-0cae942e-124f-11e8-8301-39a5d7b411a0.png">

## License

Copyright (c) 2018 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)
